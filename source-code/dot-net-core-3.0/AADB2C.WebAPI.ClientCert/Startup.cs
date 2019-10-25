using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AADB2C.WebAPI.ClientCert.Models;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AADB2C.WebAPI.ClientCert
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            var appSettings = Configuration.GetSection("AppSettings").Get<AppSettingsModel>();

            services.AddAuthentication(
                CertificateAuthenticationDefaults.AuthenticationScheme)
                .AddCertificate(options =>
                {
                    options.AllowedCertificateTypes = CertificateTypes.All;
                    options.ValidateValidityPeriod = true;
                    options.Events = new CertificateAuthenticationEvents
                    {
                        OnCertificateValidated = context =>
                        {
                            // Check the thumbprint of the certificate
                            if ((!string.IsNullOrEmpty(appSettings.ClientCertificateThumbprint)) && context.ClientCertificate.Thumbprint.ToLower() != appSettings.ClientCertificateThumbprint.ToLower())
                            {
                                context.Fail($"Thumbprint '{context.ClientCertificate.Thumbprint.ToUpper()}' is not valid");
                                return Task.CompletedTask;
                            }

                            // Check the issuer name of the certificate
                            if ((!string.IsNullOrEmpty(appSettings.ClientCertificateIssuer)) && context.ClientCertificate.Issuer.ToLower() != appSettings.ClientCertificateIssuer.ToLower())
                            {
                                context.Fail($"Issuer '{context.ClientCertificate.Issuer}' is not valid");
                                return Task.CompletedTask;
                            }

                            // Check the subject of the certificate
                            if ((!string.IsNullOrEmpty(appSettings.ClientCertificateSubject)) && context.ClientCertificate.Subject.ToLower() != appSettings.ClientCertificateSubject.ToLower())
                            {
                                context.Fail($"Subject '{context.ClientCertificate.Subject}' is not valid");
                                return Task.CompletedTask;
                            }

                            context.Success();
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
