using System;
using System.Collections.Generic;

namespace Ex1_VacuumWorld
{
    class Room
    {
        public readonly int Identificador;
        public readonly string Nome;
        public bool EstaLimpa { get; private set; }
        private readonly List<Room> _neighboors;

        public Room(int identificador, string nome, bool estaLimpa)
        {
            Identificador = identificador;
            Nome = nome;
            EstaLimpa = estaLimpa;
            _neighboors = new List<Room>();
        }

        public void Limpar()
        {
            EstaLimpa = true;
        }

        public void AddNeighboor(Room room)
        {
            if (!_neighboors.Contains(room))
            {
                _neighboors.Add(room);
                room.AddNeighboor(this);
            }
        }

        public List<Room> GetNeighboors()
        {
            return _neighboors;
        }

        public override bool Equals(object obj)
        {
            if (obj is Room room2)
                return room2.Identificador == Identificador
                    && room2.EstaLimpa == EstaLimpa;
            else
                return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Identificador, EstaLimpa);
        }

        public void Act()
        {
            Limpar();
        }

        public List<Room> GetActions()
        {
            var actions = new List<Room>();
            if (!EstaLimpa)
                actions.Add(this);
            else
                actions.AddRange(GetNeighboors());

            return actions;
        }
    }
}
