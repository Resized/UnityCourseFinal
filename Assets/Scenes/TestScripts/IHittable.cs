
public interface IHittable : ITargetable
{
    public float healthPoints { get; set; }
    public HealthManager _healthManager { get; set; }
    public abstract void ProcessHit(float damage);

}
