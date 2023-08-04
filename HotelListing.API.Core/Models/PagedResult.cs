namespace HotelListingAPI.Models
{
  
        public class PagedResult<T>
        {
            //How many records are there
            public int TotalCount { get; set; }

            //What page are you
            public int PageNumber { get; set; }

            public int RecordNumber { get; set; }

            
            public List<T> Items { get; set; }

        }

    
}
