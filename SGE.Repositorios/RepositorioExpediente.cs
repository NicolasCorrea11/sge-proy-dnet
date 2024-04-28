﻿using SGE.Aplicacion;
using System.ComponentModel;
using System.IO;
namespace SGE.Repositorios;

public class RepositorioExpediente: IExpedienteRepositorio
{
    readonly string _nomarch = "expedientes.txt";
    public void AltaExpediente(Expediente e, int IdUser)
    {
        ServicioAutorizacionProvisiorio val = new ServicioAutorizacionProvisiorio();
        int id = 1;
        using var sw = new StreamWriter(_nomarch, true);
        if (val.PoseeElPermiso(IdUser, Permiso.ExpedienteAlta))
        {
            sw.WriteLine(id);
            sw.WriteLine(e.Caratula);
            sw.WriteLine(e.FechayHoraCr);
            sw.WriteLine(e.FechayHoraMod);
            sw.WriteLine(e.IdUltMod);
            sw.WriteLine(e.Estado);
        }
        else
        {
            using var cont = new StreamReader(_nomarch);
            int filas = 0;
            while (cont.ReadLine() != null)
            {
                filas++;
            }
            using var srr = new StreamReader(_nomarch);
            for (int i = 0; i < filas-6; i++)
            {
                srr.ReadLine();
            }
            id = int.Parse(srr.ReadLine() ?? "");
            id++;
            using var stw = new StreamWriter(_nomarch, true);
            stw.WriteLine(id);
            stw.WriteLine(e.Caratula);
            stw.WriteLine(e.FechayHoraCr);
            stw.WriteLine(e.FechayHoraMod);
            stw.WriteLine(IdUser);
            stw.WriteLine(e.Estado);
        }
    }

    public List<Expediente> ListarExps()
    {
        using var sr = new StreamReader(_nomarch);
        var resultado = new List<Expediente>();
        while (!sr.EndOfStream)
        {
            var expe = new Expediente();
            expe.Id = int.Parse(sr.ReadLine() ?? "");
            expe.Caratula = sr.ReadLine() ?? "";
            expe.FechayHoraCr = DateTime.Parse(sr.ReadLine() ?? "");
            expe.FechayHoraMod = DateTime.Parse(sr.ReadLine() ?? "");
            expe.IdUltMod = int.Parse(sr.ReadLine() ?? "");
            resultado.Add(expe);
        }
        return resultado;
    }

    public void BajaExpediente(int idExp, int idUser)
    {
        ServicioAutorizacionProvisiorio val = new ServicioAutorizacionProvisiorio();
        if (val.PoseeElPermiso(idUser, Permiso.ExpedienteBaja))
        {
            List<Expediente> lista = ListarExps();
            File.WriteAllText(_nomarch, "");
            foreach (Expediente exp in lista)
            {
                List<Expediente> lista2 = ListarExps();
                File.WriteAllText(_nomarch, "");
                foreach (Expediente expe in lista)
                {
                    if (exp.Id != idExp)
                    {
                        AltaExpediente(exp, idUser);
                    }   
                }
            }
        }
        else
        {
            throw new AutorizacionException("No se tienen los permisos necesarios");
        }
    }

    public void ModificarExpediente(Expediente e, int idUser)
    {
        ServicioAutorizacionProvisiorio val = new ServicioAutorizacionProvisiorio();
        if (val.PoseeElPermiso(idUser, Permiso.ExpedienteModificacion))
        {
            List<Expediente> lista = ListarExps();
            int i = 0;
            while (lista[i].Id != e.Id)
            {
                i++;
            }
            lista[i] = e;
            File.WriteAllText(_nomarch, "");
            using var sw = new StreamWriter(_nomarch, true);
            foreach (Expediente exp in lista)
            {
                sw.WriteLine(e.Id);
                sw.WriteLine(e.Caratula);
                sw.WriteLine(e.FechayHoraCr);
                sw.WriteLine(e.FechayHoraMod);
                sw.WriteLine(e.IdUltMod);
                sw.WriteLine(e.Estado);
                sw.Close();
            }
        }
    }

    public Expediente ConsultaPorId(int id)
    {
        var exp = new Expediente();
        var lista = ListarExps();
        foreach (Expediente e in lista)
        {
            if (id == e.Id)
            {
                exp = e;
            }
        }
        return exp;
    }
}
