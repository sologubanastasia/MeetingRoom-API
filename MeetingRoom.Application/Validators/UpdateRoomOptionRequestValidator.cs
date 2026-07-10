using FluentValidation;
using MeetingRoom.Application.Dtos.Rooms;

namespace MeetingRoom.Application.Validators.Rooms
{
    public class UpdateRoomOptionRequestValidator : AbstractValidator<UpdateRoomOptionRequest>
    {
        public UpdateRoomOptionRequestValidator()
        {
            RuleFor(option => option.Name)
                .NotEmpty()
                .WithMessage("Назва послуги є обов'язковою")
                .MaximumLength(100)
                .WithMessage("Назва послуги не може перевищувати 100 символів");

            RuleFor(option => option.Price)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Ціна послуги не може бути від'ємною");
        }
    }
}
