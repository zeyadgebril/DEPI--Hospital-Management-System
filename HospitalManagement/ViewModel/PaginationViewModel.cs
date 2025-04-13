namespace HospitalManagement.ViewModel
{
    public class PaginationViewModel<T>
    {
        public List<T> Items { get; set; } // البيانات (المرضى، الأطباء، إلخ)
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages {  get; set; }
        public string SearchTerm { get; set; }
    }
}

