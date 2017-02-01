using System;

namespace FinalYearProject
{
    abstract class Technique
    {
        abstract public void update(Client clnt, Player player, World world, string action);
        abstract public Tuple<int, int> process(Client clnt, World world);
        abstract public string getLastAction();
    }
}
