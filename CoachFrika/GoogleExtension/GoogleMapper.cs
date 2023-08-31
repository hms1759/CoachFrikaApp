using CoachFrika.Models;

namespace CoachFrika.GoogleExtension
{
    public static class GoogleMapper
    {
        public static List<ContactUs> MapFromRangeData(IList<IList<object>> values)
        {
            var items = new List<ContactUs>();

            foreach (var value in values)
            {
                ContactUs item = new()
                {
                    FullName = value[1].ToString(),
                    Email = value[2].ToString(),
                    PhoneNumber = value[3].ToString()
                };

                items.Add(item);
            }

            return items;
        }
        public static IList<IList<object>> MapToRangeData(ContactUs item)
        {
            var objectList = new List<object>() {
                item.Id,
                item.FullName,
                item.Email,
                item.PhoneNumber,
                item.SchoolName,
                item.SchoolAddress,
                item.YearsOfExperience,
                item.AreaofInterest,
                item.Plan.ToString(),
                item.AreaofInterest.ToString(),
                item.WhyInterested
            };
            var rangeData = new List<IList<object>> { objectList };
            return rangeData;
        }
        public static IList<IList<object>> MapToRangeContactData(ContactUs item)
        {
            var objectList = new List<object>() {
                item.Id,
                item.FullName,
                item.Email,
                item.PhoneNumber,
                item.SchoolName
            };
            var rangeData = new List<IList<object>> { objectList };
            return rangeData;
        }
    }
}
