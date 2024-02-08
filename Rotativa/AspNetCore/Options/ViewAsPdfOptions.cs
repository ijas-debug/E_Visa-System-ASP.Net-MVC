namespace Rotativa.AspNetCore.Options
{
    internal class ViewAsPdfOptions
    {
        public string FileName { get; set; }
        public Size PageSize { get; set; }
        public Orientation PageOrientation { get; set; }
        public Margins PageMargins { get; set; }
        public string CustomSwitches { get; set; }
    }
}