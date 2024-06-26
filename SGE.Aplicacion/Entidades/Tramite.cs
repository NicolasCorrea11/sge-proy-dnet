﻿namespace SGE.Aplicacion;

public class Tramite
{
    public int Id {set;get;}
    public int ExpedienteId {set;get;}
    public EtiquetaTramite Etiqueta {set;get;}
    public string Contenido {set;get;} = "";
    public DateTime FechayHoraCr {set;get;}
    public DateTime FechayHoraMod {set;get;}
    public int IdUser {set;get;}

    public override string ToString()
        => $"ID = {Id}, ID del expediente = {ExpedienteId}, etiqueta = {Etiqueta}, contenido = {Contenido}, fecha y hora de creacion = {FechayHoraCr}, fecha y hora de modificacion = {FechayHoraMod}, ID de ultimo usuario en modificar = {IdUser}";
}
