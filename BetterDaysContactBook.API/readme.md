// add identity
            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<BetterDaysContactBookContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(option =>
            {
                option.User.RequireUniqueEmail = true;
                option.Password.RequiredLength = 5;
            });



            // add authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudience=Configuration["JWTSettings:Audience"],
                    ValidIssuer=Configuration["JWTSettings:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey
                        (Encoding.UTF8.GetBytes(Configuration["JWTSettings:SecretKey"])),
                    ClockSkew = TimeSpan.Zero
                };
            });



app.UseSwaggerUI(x => x.SwaggerEndpoint("/swagger/v1/swagger.json", DemoAuthAPI v1));

signingCredentials: new SigningCredentials(issuerSigningKey, SecurityAlgorithms.HmacSha256)