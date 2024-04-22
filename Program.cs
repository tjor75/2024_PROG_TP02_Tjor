using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

const int OP_MIN = 1, OP_MAX = 5;
const int OP_CARGAR_PERSONA = 1;
const int OP_OBTENER_ESTADISTICAS = 2;
const int OP_BUSCAR_PERSONA = 3;
const int OP_MODIFICAR_EMAIL = 4;
const int OP_SALIR = 5;
int opcion, dni, cantPersonas, cantHabilitadosVotar, edadPromedio;
string apellido, nombre, email;
DateTime fechaNacimiento;
Dictionary<int, Persona> personas = new Dictionary<int, Persona>();

string IngresarCadena(string mensaje)
{
    Console.WriteLine(mensaje);
    return Console.ReadLine();
}
int IngresarEntero(string mensaje)
{
    int ingreso;
    bool esEntero = int.TryParse(IngresarCadena(mensaje), out ingreso);
    
    while (!esEntero)
        esEntero = int.TryParse(IngresarCadena("Error: ingrese un entero\n" + mensaje), out ingreso);

    return ingreso;
}
int IngresarEnteroPositivo(string mensaje)
{
    int ingreso = IngresarEntero(mensaje);
    while (ingreso <= 0)
        ingreso = IngresarEntero("Error: ingrese un entero positivo\n" + mensaje);
    return ingreso;
}
int IngresarEnteroEntre(string mensaje, int min, int max)
{
    int ingreso = IngresarEntero(mensaje);

    while (ingreso < min || ingreso > max)
        ingreso = IngresarEntero("Error: ingrese un entero entre " + min + "-" + max + "\n" + mensaje);
        
    return ingreso;
}
DateTime IngreseFecha(string mensaje)
{
    DateTime ingreso;
    bool esFechaValida = DateTime.TryParse(IngresarCadena(mensaje), out ingreso);
    
    while (!esFechaValida)
        esFechaValida = DateTime.TryParse(IngresarCadena("Error: ingrese una fecha válida\n" + mensaje), out ingreso);

    return ingreso;
}
DateTime IngreseFechaNoFutura(string mensaje)
{
    DateTime ingreso = IngreseFecha(mensaje);
    DateTime now = DateTime.Now;
    
    while (ingreso > now)
        ingreso = IngreseFecha("Error: se ingresó una fecha futura\n" + mensaje);

    return ingreso;
}
string IngresarEmail(string mensaje)
{
    const string PATRON = @"^[\w+\-.]+@(?!\.)[0-9A-Za-z\-.]+\.[A-Za-z]+$";
    string ingreso = IngresarCadena(mensaje);

    while(!Regex.IsMatch(ingreso, PATRON))
        ingreso = IngresarCadena("Error: Email no válido\n" + mensaje);

    return ingreso.ToLower();
}
int IngresarDNI()
{
    int dni = IngresarEnteroPositivo("Ingrese el DNI de una persona:");
    while (!personas.ContainsKey(dni))
        dni = IngresarEnteroPositivo("Error: el DNI no es válido.\nIngrese el DNI de una persona:");
    return dni;
}


int ObtenerCantHabilitadosVotar(Dictionary<int, Persona> personas)
{
    int cantHabilitadosVotar = 0;
    foreach (Persona laPersona in personas.Values)
    {
        if (laPersona.PuedeVotar())
            cantHabilitadosVotar++;
    }
    return cantHabilitadosVotar;
}
int ObtenerEdadPromedio(Dictionary<int, Persona> personas, int cantPersonas)
{
    int sumaEdades = 0;
    foreach (Persona laPersona in personas.Values)
        sumaEdades += laPersona.ObtenerEdad();
    return sumaEdades / cantPersonas;
}


void MostrarMenu()
{
    Console.WriteLine("=== SISTEMA DE FACILITACIÓN DE CENSOS ===\n" +
        "1. Cargar Nueva Persona\n" +
        "2. Obtener Estadísticas del Censo\n" +
        "3. Buscar Persona\n" +
        "4. Modificar Mail de una Persona.\n" +
        "5. Salir");
}
void MostrarEstadisticas(int cantPersonas, int cantHabilitadosVotar, int edadPromedio)
{
    Console.WriteLine("Estadísitcas del Censo:\n" +
        "Cantidad de Personas: " + cantPersonas + "\n" +
        "Cantidad de Personas habilitadas para votar: " + cantHabilitadosVotar + "\n" +
        "Promedio de Edad: " + edadPromedio);
}
void MostrarPersona(int dni, Dictionary<int, Persona> personas)
{
    Persona laPersona = personas[dni];

    Console.WriteLine(laPersona.Apellido + ", " + laPersona.Nombre + "\n" +
        "DNI: " + dni + "\n" +
        "Fecha de nacimiento: " + laPersona.FechaNacimiento.ToShortDateString() + "\n" +
        "Email:" + laPersona.Email);
}


MostrarMenu();
opcion = IngresarEnteroEntre("Ingrese una opción:", OP_MIN, OP_MAX);
while (opcion != OP_SALIR)
{
    switch (opcion)
    {
        case OP_CARGAR_PERSONA:
            dni = IngresarEnteroPositivo("Ingrese el DNI de una persona:");

            while (personas.ContainsKey(dni))
                dni = IngresarEnteroPositivo("Error: el DNI ya fue ingresado.\nIngrese el DNI de una persona:");

            apellido = IngresarCadena("Ingrese su apellido:");
            nombre = IngresarCadena("Ingrese su nombre:");
            fechaNacimiento = IngreseFechaNoFutura("Ingrese su fecha de nacimiento (YYYY-MM-DD):");
            email = IngresarEmail("Ingrese su Email:");

            personas.Add(dni, new Persona(dni, apellido, nombre, fechaNacimiento, email));

            Console.WriteLine($"Se ha creado la persona {nombre} {apellido} y se ha agregado a la lista.");
            break;
        
        case OP_OBTENER_ESTADISTICAS:
            cantPersonas = personas.Count;
            cantHabilitadosVotar = ObtenerCantHabilitadosVotar(personas);
            edadPromedio = ObtenerEdadPromedio(personas, cantPersonas);

            MostrarEstadisticas(cantPersonas, cantHabilitadosVotar, edadPromedio);
            break;

        case OP_BUSCAR_PERSONA:
            dni = IngresarDNI();
            MostrarPersona(dni, personas);
            break;

        case OP_MODIFICAR_EMAIL:
            dni = IngresarDNI();
            email = IngresarEmail("Ingrese su nuevo Email:");

            personas[dni].Email = email;

            Console.WriteLine("Email modificado.");
            break;
    }

    MostrarMenu();
    opcion = IngresarEnteroEntre("Ingrese una opción:", OP_MIN, OP_MAX);
}