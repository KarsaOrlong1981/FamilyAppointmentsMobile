using FamilyAppointmentsMobile.Helpers;

namespace FamilyAppointmentsMobile.Models
{
    public class EventModel
    {
        private Random rnd = new Random();

        public string Name { get; set; }
        public string Description { get; set; }

        public string PostItSource { get; set; }

        public TimeSpan Time { get; set; }

        public Color MemberColor => SetMemberColor();

        public string TimeFormatted => $"{Time.Hours:D2}:{Time.Minutes:D2} Uhr";

        public EventModel() 
        {
            PostItSource = SetPostIt();
        }

        private Color SetMemberColor()
        {
            switch (Name)
            {
                case "Karin": return ResourceHelper.GetResource<Color>("KarinColor");
                case "Joerg": return ResourceHelper.GetResource<Color>("JoergColor");
                case "Marvin": return ResourceHelper.GetResource<Color>("MarvinColor");
                case "Lio": return ResourceHelper.GetResource<Color>("LioColor");
                case "A.R.T.": return Colors.ForestGreen;
            }
            return Colors.White;
        }

        private string SetPostIt()
        {
            var count = rnd.Next(1, 9);
            var postIt = $"postit{count}.png";
            return postIt;
        }
    }
}
