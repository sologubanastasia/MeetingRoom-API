using FluentValidation;
using FluentValidation.AspNetCore;
using Abp.MeetingRoom.Bll;
using Abp.MeetingRoom.Dal.SqlRepositories;
using Abp.MeetingRoom.Services.Web.MappingProfiles;
using Abp.MeetingRoom.Services.Web.Middleware;
using Abp.MeetingRoom.Services.Web.Validators.RoomBookings;
var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateRoomBookingRequestValidator>();
builder.Services.AddAutoMapper(_ => { }, typeof(RoomMappingProfile).Assembly);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddBll();
builder.Services.AddSqlRepositories();
var app = builder.Build();
app.UseMiddleware<ExceptionHandlingMiddleware>();
if (app.Environment.IsDevelopment())
{
    app.Services
        .GetRequiredService<AutoMapper.IConfigurationProvider>()
        .AssertConfigurationIsValid();
    app.UseSwagger();
    app.UseSwaggerUI();
}
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
app.UseAuthorization();
app.MapControllers();
app.Run();
