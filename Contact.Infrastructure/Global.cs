﻿global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Http;
global using Contact.Domain;
global using FluentValidation;
global using Contact.Infrastructure.Helpers;
global using Contact.Infrastructure.Middleware;
global using Microsoft.Extensions.Logging;
global using System.Threading.RateLimiting;
global using Microsoft.AspNetCore.RateLimiting;