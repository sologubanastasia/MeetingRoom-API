using FluentValidation;
using MeetingRoom.Application.Dtos.Rooms;

namespace MeetingRoom.Application.Validators.Rooms
{
    public class UpdateRoomRequestValidator : AbstractValidator<UpdateRoomRequest>
    {
        public UpdateRoomRequestValidator()
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
                .WithMessage("Ціна за годину повинна бути більшою за нуль");

            RuleFor(room => room.Options).NotNull().WithMessage("Список послуг не може бути null");

            RuleForEach(room => room.Options).SetValidator(new UpdateRoomOptionRequestValidator());
        }
    }
}
