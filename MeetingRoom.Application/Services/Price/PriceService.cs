namespace MeetingRoom.Application.Services.Price
{
    /// <summary>
    /// Виконує розрахунок вартості оренди залу
    /// залежно від часу бронювання.
    /// </summary>
    public class PriceService : IPriceService
    {
        public decimal CalculateRoomPrice(
            decimal pricePerHour,
            DateTime startTime,
            DateTime endTime
        )
        {
            if (pricePerHour <= 0)
            {
                throw new ArgumentException("Price per hour must be greater than zero.");
            }

            if (startTime >= endTime)
            {
                throw new ArgumentException("Start time must be earlier than end time.");
            }

            decimal totalPrice = 0m;
            var currentTime = startTime;

            while (currentTime < endTime)
            {
                var nextTime = GetNextPriceBoundary(currentTime);
                nextTime = nextTime < endTime ? nextTime : endTime;

                var hours = (decimal)(nextTime - currentTime).TotalHours;

                var multiplier = GetPriceMultiplier(currentTime.TimeOfDay);

                totalPrice += pricePerHour * multiplier * hours;
                currentTime = nextTime;
            }

            return totalPrice;
        }

        private static DateTime GetNextPriceBoundary(DateTime currentTime)
        {
            var date = currentTime.Date;
            var boundaries = new[]
            {
                date.AddHours(6),
                date.AddHours(9),
                date.AddHours(12),
                date.AddHours(14),
                date.AddHours(18),
                date.AddHours(23),
                date.AddDays(1).AddHours(6),
            };

            return boundaries.First(boundary => boundary > currentTime);
        }

        /// <summary>
        /// Повертає тарифний множник відповідно до часу доби.
        /// </summary>
        /// <param name="time">Час доби.</param>
        /// <returns>Тарифний множник.</returns>
        private static decimal GetPriceMultiplier(TimeSpan time)
        {
            if (time >= TimeSpan.FromHours(12) && time < TimeSpan.FromHours(14))
            {
                return 1.15m;
            }

            if (time >= TimeSpan.FromHours(6) && time < TimeSpan.FromHours(9))
            {
                return 0.90m;
            }

            if (time >= TimeSpan.FromHours(18) && time < TimeSpan.FromHours(23))
            {
                return 0.80m;
            }

            return 1.00m;
        }
    }
}
