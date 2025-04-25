namespace ClinicHub.Web.Services;
public class PatientService
{
    private readonly HttpClient _http;
    public PatientService(HttpClient http) => _http = http;
    
    public async Task<Patient[]> GetPatientsAsync() 
    {
        return await _http.GetFromJsonAsync<Patient[]>("api/patients");
    }
}