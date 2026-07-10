namespace MeetingRoom.Application.Services.Price
{
    /// <summary>
    /// Визначає операції для розрахунку вартості оренди залу.
    /// </summary>
    public interface IPriceService
    {
        /// <summary>
        /// Розраховує вартість оренди залу з урахуванням часу бронювання.
        /// </summary>
        /// <param name="pricePerHour">Базова вартість оренди за годину.</param>
        /// <param name="startTime">Дата і час початку бронювання.</param>
        /// <param name="endTime">Дата і час завершення бронювання.</param>
        /// <returns>Розрахована вартість оренди залу.</returns>
        decimal CalculateRoomPrice(decimal pricePerHour, DateTime startTime, DateTime endTime);
    }
}
