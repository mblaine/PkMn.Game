
namespace PkMn.Game.SpecialEffects
{
    public interface ISpecialEffect
    {
        void Begin();

        void Step(int frameCounter);

        bool IsOver(int frameCounter);

        void End();
    }
}
