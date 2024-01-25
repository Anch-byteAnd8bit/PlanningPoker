namespace PlanningPoker.Models;
public class Sample
{
    public int Id { get; set; }

    public string Value { get; set; } = null!;
   
}

public class SampleServ
{
    private PockerDBContext context;

    public SampleServ(PockerDBContext _context)
    {
        _context = context;
    }

}
