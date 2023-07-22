public interface IMovement
{
    void Move();
    void Stop();
    void Rotation();
    public bool IsMoving { get; set; }
}