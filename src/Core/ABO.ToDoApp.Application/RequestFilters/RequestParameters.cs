namespace ABO.ToDoApp.Application.RequestFilters;

public abstract class RequestParameters
{
    const int _maxPageSize = 30;
    public int PageNumber { get; set; } = 1;
    private int _pageSize = 10;
    public int PageSize
    {
        get
        {
            return _pageSize;
        }
        set
        {
            _pageSize = (value > _maxPageSize) ? _maxPageSize : value;
        }
    }
}
