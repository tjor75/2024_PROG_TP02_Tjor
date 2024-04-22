using System;

class Persona {
    public int DNI { get; private set; }
    public string Apellido { get; private set; }
    public string Nombre { get; private set; }
    public DateTime FechaNacimiento { get; private set; }
    public string Email { get; set; }

    public Persona(int dni, string apellido, string nombre, DateTime fechaNacimiento, string email)
    {
        DNI = dni;
        Apellido = apellido;
        Nombre = nombre;
        FechaNacimiento = fechaNacimiento;
        Email = email;
    }

    public bool PuedeVotar()
    {
        const int EDAD_MIN = 16;
        return ObtenerEdad() >= EDAD_MIN;
    }

    public int ObtenerEdad()
    {
        int edad;
        DateTime hoy = DateTime.Today;

        edad = hoy.Year - FechaNacimiento.Year;
        if (hoy.Month < FechaNacimiento.Month || hoy.Month == FechaNacimiento.Month && hoy.Day < FechaNacimiento.Day)
            edad--;

        return edad;
    }
}