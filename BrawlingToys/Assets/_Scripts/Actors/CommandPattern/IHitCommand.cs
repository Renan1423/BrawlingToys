namespace BrawlingToys.Actors
{
    public interface IHitCommand
    {
        public void Execute(Hitable target);
        public Bullet GetBullet();
        public void SetBullet(Bullet bullet);
    }
}