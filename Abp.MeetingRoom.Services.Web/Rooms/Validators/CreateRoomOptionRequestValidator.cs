using FluentValidation;
using Abp.MeetingRoom.Services.Web.Rooms.Dtos;
namespace Abp.MeetingRoom.Services.Web.Rooms.Validators
{
    public class CreateRoomOptionRequestValidator : AbstractValidator<CreateRoomOptionRequest>
    {
        public CreateRoomOptionRequestValidator()
        {
            RuleFor(option => option.Name)
                .NotEmpty()
                .WithMessage("Назва послуги є обов'язковою")
                .MaximumLength(100)
                .WithMessage("Назва послуги не може перевищувати 100 символів");
            RuleFor(option => option.Price)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Ціна послуги не може бути від'ємною")
                .PrecisionScale(18, 2, false)
                .WithMessage("Ціна послуги повинна містити не більше 16 цифр до коми та 2 після коми");
        }
    }
}
