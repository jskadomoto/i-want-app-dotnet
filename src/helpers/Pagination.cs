public class Pagination
{
  public static int SkipPage(int page, int rows)
  {
    return (page - 1) * rows;
  }
}