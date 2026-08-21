using FluentValidation;
using MeetingRoom.Application.Dtos.RoomBookings;

namespace MeetingRoom.Application.Validators.RoomBookings
{
    public class CreateRoomBookingRequestValidator : AbstractValidator<CreateRoomBookingRequest>
    {
        public CreateRoomBookingRequestValidator()
        {
            RuleFor(booking => booking.RoomId).NotEmpty().WithMessage("ID залу є обов'язковим");

            RuleFor(booking => booking.StartTime)
                .NotEmpty()
                .WithMessage("Час початку є обов'язковим")
                .Must(startTime => startTime.Kind == DateTimeKind.Utc)
                .WithMessage("Час початку повинен бути в UTC")
                .Must(startTime => startTime >= DateTime.UtcNow)
                .WithMessage("Час початку не можу бути в минулому");

            RuleFor(booking => booking.EndTime)
                .NotEmpty()
                .WithMessage("Час завершення є обов'язковим")
                .Must(endTime => endTime.Kind == DateTimeKind.Utc)
                .WithMessage("Час завершення повинен бути в UTC")
                .GreaterThan(booking => booking.StartTime)
                .WithMessage("Час завершення повинен бути пізніше часу початку");

            RuleFor(booking => booking.SelectedOptionIds)
                .NotNull()
                .WithMessage("Список обраних послуг не може бути null");

            RuleForEach(booking => booking.SelectedOptionIds)
                .NotEmpty()
                .WithMessage("ID послуги не може бути порожнім");

            RuleFor(booking => booking.SelectedOptionIds)
                .Must(ids => ids.Count <= 3)
                .WithMessage("Можливо обрати не більше трьох додаткових послуг")
                .When(booking => booking.SelectedOptionIds is not null);

            RuleFor(booking => booking.SelectedOptionIds)
                .Must(HaveUniqueIds)
                .WithMessage("ID послуг не повинні повторюватися")
                .When(booking => booking.SelectedOptionIds is not null);
        }

        private static bool HaveUniqueIds(List<Guid> ids)
        {
            return ids.Distinct().Count() == ids.Count;
        }
    }
}
