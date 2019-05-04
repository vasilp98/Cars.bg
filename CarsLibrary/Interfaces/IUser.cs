namespace Cars.bg
{
    public interface IUser
    {
        string cars { get; set; }
        string gender { get; set; }
        string messages { get; set; }
        string password { get; set; }
        string type { get; set; }
        string username { get; set; }

        void addCar();
        void deleteCar();
        void deleteMyProfile();
        void searchCar();
        void seeAllNewMessages();
        void show();
        void writeMessage();
    }
}