namespace ZBlazor.Pages
{
    public interface IZMachineModel
    {
        string Output { get; set; } //= new string('#', 10000)  + Environment.NewLine;
        string CurrentRoom { get; set; }
        string Score { get; set; }
        string Turns { get; set; }
    }
}