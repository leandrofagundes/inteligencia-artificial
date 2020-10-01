using System;
using System.Collections.Generic;
using System.Linq;

namespace Ex1_VacuumWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Ex1: Aspirador de pó");

            var estadoInicial = CriarEstadoInicial();
            var estadoDesejado = CriarEstadoDesejado();

            ExecutaBusca(estadoInicial, estadoDesejado);

            Console.ReadKey();
        }

        private static List<Tuple<string, bool, bool, bool>> CriarEstadoInicial()
        {
            var estadoInicial = new List<Tuple<string, bool, bool, bool>>();
            estadoInicial.Add(new Tuple<string, bool, bool, bool>("sala1", false, false, false));
            return estadoInicial;
        }

        private static List<Tuple<string, bool, bool, bool>> CriarEstadoDesejado()
        {
            var estadoDesejado = new List<Tuple<string, bool, bool, bool>>();
            estadoDesejado.Add(new Tuple<string, bool, bool, bool>("sala1", true, true, true));
            estadoDesejado.Add(new Tuple<string, bool, bool, bool>("sala2", true, true, true));
            estadoDesejado.Add(new Tuple<string, bool, bool, bool>("sala3", true, true, true));
            return estadoDesejado;
        }

        private static void ExecutaBusca(
            List<Tuple<string, bool, bool, bool>> estadoInicial,
            List<Tuple<string, bool, bool, bool>> estadosDesejados)
        {
            var estadoAtual = estadoInicial.FirstOrDefault();
            Tuple<string, bool, bool, bool> estadoAnterior = null;

            Console.WriteLine("Robô está na " + estadoAtual.Item1);
            var visitados = new Stack<Tuple<string, bool, bool, bool>>();
            var pendentes = new Stack<Tuple<string, bool, bool, bool>>();
            pendentes.Push(estadoAtual);

            while (pendentes.Any())
            {
                estadoAtual = pendentes.Pop();
                visitados.Push(estadoAtual);

                if (estadoAnterior != null && estadoAtual.Item1 == estadoAnterior.Item1)
                    Console.WriteLine("Limpar " + estadoAtual.Item1);
                else if (estadoAnterior != null)
                    Console.WriteLine("Ir para " + estadoAtual.Item1);

                if (estadosDesejados.Any(
                    x => x.Item1 == estadoAtual.Item1
                    && x.Item2 == estadoAtual.Item2
                    && x.Item3 == estadoAtual.Item3
                    && x.Item4 == estadoAtual.Item4))
                    return;

                var acoesPossiveis = GetActions(estadoAtual);
                foreach (var acaoPossivel in acoesPossiveis)
                {
                    //se a ação possível for na mesma sala, é uma limpeza. adiciono apenas ela e vou adiante
                    if (acaoPossivel.Item1 == estadoAtual.Item1)
                    {
                        pendentes.Push(acaoPossivel);
                        break;
                    }
                    else
                    {
                        //se não tem limpeza, monto como ação as próximas salas
                        if (!visitados.Contains(acaoPossivel))
                            pendentes.Push(acaoPossivel);
                    }
                }

                estadoAnterior = estadoAtual;
            }
        }

        private static List<Tuple<string, bool, bool, bool>> GetActions(Tuple<string, bool, bool, bool> estadoAtual)
        {
            var acoes = new List<Tuple<string, bool, bool, bool>>();

            if (estadoAtual.Item1 == "sala1")
            {
                if (estadoAtual.Item2 == false)
                    acoes.Add(new Tuple<string, bool, bool, bool>(estadoAtual.Item1, true, estadoAtual.Item3, estadoAtual.Item4));

                acoes.Add(new Tuple<string, bool, bool, bool>("sala2", estadoAtual.Item2, estadoAtual.Item3, estadoAtual.Item4));
                acoes.Add(new Tuple<string, bool, bool, bool>("sala3", estadoAtual.Item2, estadoAtual.Item3, estadoAtual.Item4));
            }
            else if (estadoAtual.Item1 == "sala2")
            {
                if (estadoAtual.Item3 == false)
                    acoes.Add(new Tuple<string, bool, bool, bool>(estadoAtual.Item1, estadoAtual.Item2, true, estadoAtual.Item4));

                acoes.Add(new Tuple<string, bool, bool, bool>("sala1", estadoAtual.Item2, estadoAtual.Item3, estadoAtual.Item4));
                acoes.Add(new Tuple<string, bool, bool, bool>("sala3", estadoAtual.Item2, estadoAtual.Item3, estadoAtual.Item4));
            }
            else if (estadoAtual.Item1 == "sala3")
            {
                if (estadoAtual.Item4 == false)
                    acoes.Add(new Tuple<string, bool, bool, bool>(estadoAtual.Item1, estadoAtual.Item2, estadoAtual.Item3, true));

                acoes.Add(new Tuple<string, bool, bool, bool>("sala2", estadoAtual.Item2, estadoAtual.Item3, estadoAtual.Item4));
                acoes.Add(new Tuple<string, bool, bool, bool>("sala1", estadoAtual.Item2, estadoAtual.Item3, estadoAtual.Item4));
            }

            return acoes;
        }
    }
}
