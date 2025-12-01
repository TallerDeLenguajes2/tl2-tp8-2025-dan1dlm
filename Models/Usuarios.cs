namespace tl2_tp8_2025_dan1dlm.Models;

public class Usuarios
{
    private int id {get; set;}
    private string nombre {get; set;}
    private string user {get; set;}
    private string pass {get; set;}
    private string rol {get; set;}

    public class Usuario(){} // constructor

    public int Id {get => id; set => id = value;}
    public string Nombre {get => nombre; set => nombre = value;}
    public string User {get => user; set => user = value;}
    public string Pass {get => pass; set => pass = value;}
    public string Rol {get => rol; set => rol = value;}
}