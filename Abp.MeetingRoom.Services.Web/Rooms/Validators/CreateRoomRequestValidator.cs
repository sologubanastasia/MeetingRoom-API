using FluentValidation;
using Abp.MeetingRoom.Services.Web.Rooms.Dtos;
namespace Abp.MeetingRoom.Services.Web.Rooms.Validators
{
    public class CreateRoomRequestValidator : AbstractValidator<CreateRoomRequest>
    {
        public CreateRoomRequestValidator()
        {
            RuleFor(room => room.Name)
                .NotEmpty()
                .WithMessage("Назва залу є обов'язковою")
                .MaximumLength(100)
                .WithMessage("Назва залу не може перевищувати 100 символів");
            RuleFor(room => room.Capacity)
                .GreaterThan(0)
                .WithMessage("Місткість повинна бути більшою за нуль");
            RuleFor(room => room.PricePerHour)
                .GreaterThan(0)
                .WithMessage("Ціна за годину повинна бути більшою за нуль")
                .PrecisionScale(18, 2, false)
                .WithMessage("Ціна за годину повинна містити не більше 16 цифр до коми та 2 після коми");
            RuleFor(room => room.Options).NotNull().WithMessage("Список послуг не може бути null");
            RuleForEach(room => room.Options).SetValidator(new CreateRoomOptionRequestValidator());
            RuleFor(room => room.Options)
                .Must(HaveUniqueNames)
                .WithMessage("Назви послуг не повинні повторюватися")
                .When(room => room.Options is not null);
        }
        private static bool HaveUniqueNames(List<CreateRoomOptionRequest> options)
        {
            return options
                    .Select(option => option.Name?.Trim().ToLowerInvariant())
                    .Distinct()
                    .Count() == options.Count;
        }
    }
}
