namespace Musical_Theatre.Constants
{
    public class ErrorMessages
    {
        public const string ErrorViewFilePath = "~/Views/Home/Error.cshtml";

        public const string InvalidHallName = "This Hall does not exist Please enter a valid Hall Name.";
        public const string EmptyPerformances = "There is no Performances, please first add a Performance";
        public const string EmptyPerformance = "There is no Performance with such an Id, please enter a valid Performance Id.";
        public const string CreationError = "There was an error during the creation process, please make sure all the data fields have been filled correctly.";
        public const string DataTransferError = "There was an error while updating the database! Please, try again.";
        public const string EditingError = "There was an error during the editing process, please make sure all the data fields have been filled correctly.";
        public const string DeletionError = "There was an error while deleting from the database! Please, try again.";
        public const string AccsessingError = "There was an error while accessing the database! Please, try again.";
        public const string EmptyHalls = "There are no existing Halls, please enter a valid Hall Id.";
        public const string EmptyHall = "There is no Hall with such an Id, please first add a Hall.";
        public const string WrongSeatError = "There is no Valid Seat with such a Row or Seat Number for this Performance. Please check the Hall Layout for both the size of the Hall and which Seats are taken and select a valid Seat.";


    }
}
