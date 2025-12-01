using tl2_tp8_2025_dan1dlm.Models;

namespace tl2_tp8_2025_dan1dlm.Interfaces;

public interface IUsuariosRepository
{
    Usuarios GetUser(string User, string Pass);
}