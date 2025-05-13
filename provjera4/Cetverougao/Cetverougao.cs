namespace provjera4;

public class Cetverougao
{
  public double Side1 { get; set; }
  public double Side2 { get; set; }
  public double Side3 { get; set; }
  public double Side4 { get; set; }

  public Cetverougao(double s1, double s2, double s3, double s4)
  {
    Side1 = s1;
    Side2 = s2;
    Side3 = s3;
    Side4 = s4;
  }

  public double Obim()
  {
    return Side1 + Side2 + Side3 + Side4;
  }
}

public interface IPrintable
{
  string PrintInfo();
}

public interface IShape
{
  double CalculateArea();
}
