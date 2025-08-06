namespace MiCampus.Dtos.Publication
{
    public class PublicationEditDto 
    {
        public string UserId { get; set; }  //podemos usar el token despues 
        public string TypeId { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
    }
}
