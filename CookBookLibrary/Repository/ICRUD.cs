namespace CookBookLibrary.Repository;

public interface ICrud<T> 
{ 
    public T Create(T item); 
    public T Delete(int id); 
    public T Update(T newItem, int id); 
    public T GetById(int id); 
    public List<T> GetAll();
}
