using System;
using System.Text;
using System.Threading.Tasks;
using AskMe.Data.DbContexts;
using AskMe.Data.Repository;
using AskMe.Domain.Interfaces;
using AskMe.Domain.Services;
using AskMeAPI.Helpers;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;


namespace AskMeAPI
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  builder =>
                                  {
                                      builder.WithOrigins("http://localhost:4200");
                                      builder.AllowCredentials();
                                      builder.AllowAnyHeader();
                                      builder.AllowAnyMethod();
                                  });
            });
            //services.AddCors();
            services.AddControllers(setupAction =>
            {
                setupAction.ReturnHttpNotAcceptable = true;
            })
            // To support xml input and output data requests
              //.AddXmlDataContractSerializerFormatters()
              .AddNewtonsoftJson(setupAction =>
              {
                  setupAction.SerializerSettings.ContractResolver =
                    new CamelCasePropertyNamesContractResolver();
              })
              //fill out fields missing for validation responses
              .ConfigureApiBehaviorOptions(setupAction =>
              {
                  setupAction.InvalidModelStateResponseFactory = context =>
                  {
                      //create a problems details object
                      var problemDetailsFactory = context.HttpContext.RequestServices
                      .GetRequiredService<ProblemDetailsFactory>();

                      var problemDetails = problemDetailsFactory.CreateValidationProblemDetails(
                            context.HttpContext,
                            context.ModelState);
                      //dd aditional info not added by default
                      problemDetails.Detail = "See the errors field for details.";
                      problemDetails.Instance = context.HttpContext.Request.Path;

                      //find out which status code to use
                      var actionExecutingContext =
                        context as Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext;

                      //if there are moelstate errors & all arguments were correctly
                      //found/parsed we are dealing with validation errors
                      if ((context.ModelState.ErrorCount > 0) &&
                          (actionExecutingContext?.ActionArguments.Count ==
                          context.ActionDescriptor.Parameters.Count))
                      {
                          problemDetails.Type = "https://locahost/modelvalidationproblem";
                          problemDetails.Status = StatusCodes.Status422UnprocessableEntity;
                          problemDetails.Title = "One or more validation errors ocurred.";
                      }

                      return new UnprocessableEntityObjectResult(problemDetails)
                      {
                          ContentTypes = { "application/problems+json" }
                      };

                  };
              });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // configure strongly typed settings objects
            var appSettingsSection = _configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                        var userId = int.Parse(context.Principal.Identity.Name);
                        var user = userService.GetUserById(userId);
                        if (user == null)
                        {
                            // return unauthorized if user no longer exists
                            context.Fail("Unauthorized");
                        }
                        return Task.CompletedTask;
                    }
                };
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            //dependency injection resolver
            services.AddScoped<IAskMeRepository, AskMeRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISubjectService, SubjectService>();
            services.AddScoped<ILessonService, LessonService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IAnswerService, AnswerService>();
            services.AddScoped<IExamService, ExamService>();
            services.AddScoped<IResultService, ResultService>();

            services.AddDbContext<AskMeDbContext>(options =>
            {
                options.UseSqlServer(
                    @"Server=DESKTOP-F8L5PAI\SQLEXPRESS;Database=AskMe;Trusted_Connection=True;");               
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AskMeDbContext askMeDbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("An expected fault happened.");
                    });
                });
            }

            // migrate any database changes on startup (includes initial db creation)
            askMeDbContext.Database.Migrate();

            //app.UseHttpsRedirection();

            app.UseRouting();
            // global cors policy
            //app.UseCors(x => x
            //    .AllowAnyOrigin()
            //    .AllowAnyMethod()
            //    .AllowAnyHeader());
            app.UseCors(MyAllowSpecificOrigins);
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
