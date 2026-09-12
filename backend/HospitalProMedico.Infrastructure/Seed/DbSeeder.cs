using HospitalProMedico.Application.Interfaces;
using HospitalProMedico.Domain.Entities;
using HospitalProMedico.Domain.Enums;
using HospitalProMedico.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HospitalProMedico.Infrastructure.Seed;

public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<AppDbContext>();
        var passwordHasher = serviceProvider.GetRequiredService<IPasswordHasher>();

        if (await context.Users.AnyAsync())
        {
            return;
        }

        // Hospital
        var hospital = new Hospital
        {
            Name = "Hospital ProMédico",
            Cnpj = "12.345.678/0001-90",
            Phone = "(11) 3456-7890",
            Email = "contato@hospitalpromedico.com.br",
            Address = "Av. Paulista, 1000 - São Paulo/SP"
        };
        context.Hospitals.Add(hospital);

        // Health Plans
        var healthPlans = new List<HealthPlan>
        {
            new() { Name = "Amil Saúde", Code = "AMIL", Cnpj = "01.111.111/0001-01", Phone = "(11) 4000-1111", Email = "contato@amil.com.br", Address = "Rua das Palmeiras, 100", Type = HealthPlanType.PREMIUM, Status = RegisterStatus.ATIVO },
            new() { Name = "Bradesco Saúde", Code = "BRAD", Cnpj = "02.222.222/0001-02", Phone = "(11) 4000-2222", Email = "contato@bradescosaude.com.br", Address = "Av. Brigadeiro, 200", Type = HealthPlanType.INTERMEDIARIO, Status = RegisterStatus.ATIVO },
            new() { Name = "SulAmérica Saúde", Code = "SULA", Cnpj = "03.333.333/0001-03", Phone = "(11) 4000-3333", Email = "contato@sulamerica.com.br", Address = "Rua Augusta, 300", Type = HealthPlanType.PREMIUM, Status = RegisterStatus.ATIVO },
            new() { Name = "Unimed", Code = "UNIM", Cnpj = "04.444.444/0001-04", Phone = "(11) 4000-4444", Email = "contato@unimed.com.br", Address = "Rua Consolação, 400", Type = HealthPlanType.BASICO, Status = RegisterStatus.ATIVO },
            new() { Name = "Hapvida NotreDame", Code = "HAPV", Cnpj = "05.555.555/0001-05", Phone = "(11) 4000-5555", Email = "contato@hapvida.com.br", Address = "Av. Faria Lima, 500", Type = HealthPlanType.EMPRESARIAL, Status = RegisterStatus.ATIVO }
        };
        context.HealthPlans.AddRange(healthPlans);

        // Doctors
        var doctors = new List<Doctor>
        {
            new() { Name = "Dr. Carlos Alberto Souza", Crm = "CRM/SP 111111", Specialty = Specialty.CARDIOLOGIA, Cpf = "11111111111", Phone = "(11) 98888-1111", Email = "carlos.souza@hospitalpromedico.com.br", BirthDate = new DateTime(1975, 3, 12), Address = "Rua A, 10", Status = RegisterStatus.ATIVO },
            new() { Name = "Dra. Fernanda Lima", Crm = "CRM/SP 222222", Specialty = Specialty.PEDIATRIA, Cpf = "22222222222", Phone = "(11) 98888-2222", Email = "fernanda.lima@hospitalpromedico.com.br", BirthDate = new DateTime(1982, 7, 25), Address = "Rua B, 20", Status = RegisterStatus.ATIVO },
            new() { Name = "Dr. Ricardo Nogueira", Crm = "CRM/SP 333333", Specialty = Specialty.ORTOPEDIA, Cpf = "33333333333", Phone = "(11) 98888-3333", Email = "ricardo.nogueira@hospitalpromedico.com.br", BirthDate = new DateTime(1978, 11, 3), Address = "Rua C, 30", Status = RegisterStatus.ATIVO },
            new() { Name = "Dra. Juliana Martins", Crm = "CRM/SP 444444", Specialty = Specialty.GINECOLOGIA, Cpf = "44444444444", Phone = "(11) 98888-4444", Email = "juliana.martins@hospitalpromedico.com.br", BirthDate = new DateTime(1985, 5, 18), Address = "Rua D, 40", Status = RegisterStatus.ATIVO },
            new() { Name = "Dr. Marcelo Andrade", Crm = "CRM/SP 555555", Specialty = Specialty.CLINICA_GERAL, Cpf = "55555555555", Phone = "(11) 98888-5555", Email = "marcelo.andrade@hospitalpromedico.com.br", BirthDate = new DateTime(1970, 1, 30), Address = "Rua E, 50", Status = RegisterStatus.ATIVO }
        };
        context.Doctors.AddRange(doctors);

        await context.SaveChangesAsync();

        // Patients
        var patients = new List<Patient>
        {
            new() { Name = "João Pedro Alves", Cpf = "10000000001", BirthDate = new DateTime(1990, 4, 15), Gender = Gender.MASCULINO, Phone = "(11) 97777-0001", Email = "joao.alves@email.com", Address = "Rua das Flores, 10", City = "São Paulo", State = "SP", ZipCode = "01000-000", SusCardNumber = "898000000000001", HealthPlanId = healthPlans[0].Id, HealthPlanCardNumber = "AMIL0001", BloodType = BloodType.O_POSITIVO, Allergies = "Nenhuma", Status = RegisterStatus.ATIVO },
            new() { Name = "Maria Eduarda Souza", Cpf = "10000000002", BirthDate = new DateTime(1985, 8, 22), Gender = Gender.FEMININO, Phone = "(11) 97777-0002", Email = "maria.souza@email.com", Address = "Rua das Rosas, 20", City = "São Paulo", State = "SP", ZipCode = "01001-000", SusCardNumber = "898000000000002", HealthPlanId = healthPlans[1].Id, HealthPlanCardNumber = "BRAD0002", BloodType = BloodType.A_POSITIVO, Allergies = "Dipirona", Status = RegisterStatus.ATIVO },
            new() { Name = "Pedro Henrique Costa", Cpf = "10000000003", BirthDate = new DateTime(2001, 1, 5), Gender = Gender.MASCULINO, Phone = "(11) 97777-0003", Email = "pedro.costa@email.com", Address = "Av. Central, 30", City = "Campinas", State = "SP", ZipCode = "13000-000", SusCardNumber = "898000000000003", HealthPlanId = null, BloodType = BloodType.B_NEGATIVO, Allergies = "Nenhuma", Status = RegisterStatus.ATIVO },
            new() { Name = "Ana Clara Ferreira", Cpf = "10000000004", BirthDate = new DateTime(1995, 12, 30), Gender = Gender.FEMININO, Phone = "(11) 97777-0004", Email = "ana.ferreira@email.com", Address = "Rua do Sol, 40", City = "São Paulo", State = "SP", ZipCode = "01002-000", SusCardNumber = "898000000000004", HealthPlanId = healthPlans[2].Id, HealthPlanCardNumber = "SULA0004", BloodType = BloodType.AB_POSITIVO, Allergies = "Penicilina", Status = RegisterStatus.ATIVO },
            new() { Name = "Lucas Gabriel Rocha", Cpf = "10000000005", BirthDate = new DateTime(1978, 6, 10), Gender = Gender.MASCULINO, Phone = "(11) 97777-0005", Email = "lucas.rocha@email.com", Address = "Rua da Lua, 50", City = "Guarulhos", State = "SP", ZipCode = "07000-000", SusCardNumber = "898000000000005", HealthPlanId = healthPlans[3].Id, HealthPlanCardNumber = "UNIM0005", BloodType = BloodType.O_NEGATIVO, Allergies = "Nenhuma", Status = RegisterStatus.ATIVO },
            new() { Name = "Beatriz Oliveira Santos", Cpf = "10000000006", BirthDate = new DateTime(1965, 9, 14), Gender = Gender.FEMININO, Phone = "(11) 97777-0006", Email = "beatriz.santos@email.com", Address = "Rua Estrela, 60", City = "São Paulo", State = "SP", ZipCode = "01003-000", SusCardNumber = "898000000000006", HealthPlanId = healthPlans[4].Id, HealthPlanCardNumber = "HAPV0006", BloodType = BloodType.A_NEGATIVO, Allergies = "Nenhuma", Notes = "Hipertensa", Status = RegisterStatus.ATIVO },
            new() { Name = "Gabriel Souza Lima", Cpf = "10000000007", BirthDate = new DateTime(2010, 2, 20), Gender = Gender.MASCULINO, Phone = "(11) 97777-0007", Email = "gabriel.lima@email.com", Address = "Rua Nova, 70", City = "São Paulo", State = "SP", ZipCode = "01004-000", SusCardNumber = "898000000000007", HealthPlanId = healthPlans[0].Id, HealthPlanCardNumber = "AMIL0007", BloodType = BloodType.B_POSITIVO, Allergies = "Lactose", Status = RegisterStatus.ATIVO },
            new() { Name = "Larissa Mendes Silva", Cpf = "10000000008", BirthDate = new DateTime(1999, 10, 8), Gender = Gender.FEMININO, Phone = "(11) 97777-0008", Email = "larissa.silva@email.com", Address = "Rua Antiga, 80", City = "Osasco", State = "SP", ZipCode = "06000-000", SusCardNumber = "898000000000008", HealthPlanId = null, BloodType = BloodType.O_POSITIVO, Allergies = "Nenhuma", Status = RegisterStatus.ATIVO },
            new() { Name = "Rafael Barbosa Dias", Cpf = "10000000009", BirthDate = new DateTime(1958, 3, 3), Gender = Gender.MASCULINO, Phone = "(11) 97777-0009", Email = "rafael.dias@email.com", Address = "Rua Velha, 90", City = "São Paulo", State = "SP", ZipCode = "01005-000", SusCardNumber = "898000000000009", HealthPlanId = healthPlans[1].Id, HealthPlanCardNumber = "BRAD0009", BloodType = BloodType.AB_NEGATIVO, Allergies = "Nenhuma", Notes = "Diabético tipo 2", Status = RegisterStatus.ATIVO },
            new() { Name = "Camila Ribeiro Alves", Cpf = "10000000010", BirthDate = new DateTime(1992, 7, 19), Gender = Gender.FEMININO, Phone = "(11) 97777-0010", Email = "camila.alves@email.com", Address = "Av. Nações, 100", City = "São Paulo", State = "SP", ZipCode = "01006-000", SusCardNumber = "898000000000010", HealthPlanId = healthPlans[2].Id, HealthPlanCardNumber = "SULA0010", BloodType = BloodType.A_POSITIVO, Allergies = "Nenhuma", Status = RegisterStatus.ATIVO }
        };
        context.Patients.AddRange(patients);

        await context.SaveChangesAsync();

        var today = DateTime.UtcNow.Date;

        // Appointments (10)
        var appointments = new List<Appointment>
        {
            new() { PatientId = patients[0].Id, DoctorId = doctors[0].Id, Date = today, Time = "08:00", Type = AppointmentType.PRIMEIRA_CONSULTA, Status = AppointmentStatus.CONFIRMADA, Notes = "Avaliação cardiológica de rotina" },
            new() { PatientId = patients[1].Id, DoctorId = doctors[4].Id, Date = today, Time = "09:00", Type = AppointmentType.RETORNO, Status = AppointmentStatus.AGENDADA },
            new() { PatientId = patients[2].Id, DoctorId = doctors[2].Id, Date = today, Time = "10:00", Type = AppointmentType.URGENCIA, Status = AppointmentStatus.AGENDADA, Notes = "Dor no joelho após queda" },
            new() { PatientId = patients[3].Id, DoctorId = doctors[3].Id, Date = today.AddDays(1), Time = "08:30", Type = AppointmentType.PRIMEIRA_CONSULTA, Status = AppointmentStatus.AGENDADA },
            new() { PatientId = patients[4].Id, DoctorId = doctors[0].Id, Date = today.AddDays(1), Time = "11:00", Type = AppointmentType.RETORNO, Status = AppointmentStatus.AGENDADA },
            new() { PatientId = patients[5].Id, DoctorId = doctors[4].Id, Date = today.AddDays(-2), Time = "14:00", Type = AppointmentType.RETORNO, Status = AppointmentStatus.REALIZADA, Notes = "Pressão controlada" },
            new() { PatientId = patients[6].Id, DoctorId = doctors[1].Id, Date = today.AddDays(-5), Time = "09:30", Type = AppointmentType.PRIMEIRA_CONSULTA, Status = AppointmentStatus.REALIZADA },
            new() { PatientId = patients[7].Id, DoctorId = doctors[3].Id, Date = today.AddDays(-10), Time = "15:00", Type = AppointmentType.EXAME, Status = AppointmentStatus.REALIZADA },
            new() { PatientId = patients[8].Id, DoctorId = doctors[0].Id, Date = today.AddDays(-15), Time = "10:30", Type = AppointmentType.RETORNO, Status = AppointmentStatus.CANCELADA, Notes = "Paciente remarcou" },
            new() { PatientId = patients[9].Id, DoctorId = doctors[2].Id, Date = today.AddDays(2), Time = "13:00", Type = AppointmentType.PRIMEIRA_CONSULTA, Status = AppointmentStatus.AGENDADA }
        };
        context.Appointments.AddRange(appointments);

        // Hospitalizations (5)
        var hospitalizations = new List<Hospitalization>
        {
            new() { PatientId = patients[5].Id, DoctorId = doctors[0].Id, AdmissionDate = today.AddDays(-3), ExpectedDischargeDate = today.AddDays(2), Room = "301", Bed = "A", Type = HospitalizationType.CLINICA, Diagnosis = "Crise hipertensiva", Reason = "Monitoramento cardiológico", Status = HospitalizationStatus.INTERNADO },
            new() { PatientId = patients[8].Id, DoctorId = doctors[4].Id, AdmissionDate = today.AddDays(-1), ExpectedDischargeDate = today.AddDays(3), Room = "205", Bed = "B", Type = HospitalizationType.CLINICA, Diagnosis = "Descompensação diabética", Reason = "Controle glicêmico", Status = HospitalizationStatus.INTERNADO },
            new() { PatientId = patients[2].Id, DoctorId = doctors[2].Id, AdmissionDate = today.AddDays(-7), ExpectedDischargeDate = today.AddDays(-2), DischargeDate = today.AddDays(-2), Room = "110", Bed = "A", Type = HospitalizationType.CIRURGICA, Diagnosis = "Fratura de fêmur", Reason = "Pós-operatório de cirurgia ortopédica", Status = HospitalizationStatus.ALTA },
            new() { PatientId = patients[6].Id, DoctorId = doctors[1].Id, AdmissionDate = today, ExpectedDischargeDate = today.AddDays(1), Room = "PED-02", Bed = "A", Type = HospitalizationType.OBSERVACAO, Diagnosis = "Desidratação", Reason = "Hidratação venosa", Status = HospitalizationStatus.INTERNADO },
            new() { PatientId = patients[4].Id, DoctorId = doctors[0].Id, AdmissionDate = today.AddDays(-20), ExpectedDischargeDate = today.AddDays(-15), DischargeDate = today.AddDays(-16), Room = "UTI-01", Bed = "A", Type = HospitalizationType.UTI, Diagnosis = "Infarto agudo do miocárdio", Reason = "Estabilização pós-infarto", Status = HospitalizationStatus.ALTA }
        };
        context.Hospitalizations.AddRange(hospitalizations);

        // Surgeries (3)
        var surgeries = new List<Surgery>
        {
            new() { PatientId = patients[2].Id, DoctorId = doctors[2].Id, Type = "Osteossíntese de fêmur", Date = today.AddDays(-7), Time = "07:00", Room = "Centro Cirúrgico 1", EstimatedDurationMinutes = 180, Status = SurgeryStatus.REALIZADA, Notes = "Cirurgia sem intercorrências" },
            new() { PatientId = patients[3].Id, DoctorId = doctors[3].Id, Type = "Cesariana", Date = today.AddDays(5), Time = "08:00", Room = "Centro Cirúrgico 2", EstimatedDurationMinutes = 60, Status = SurgeryStatus.AGENDADA },
            new() { PatientId = patients[9].Id, DoctorId = doctors[0].Id, Type = "Cateterismo cardíaco", Date = today.AddDays(3), Time = "09:00", Room = "Hemodinâmica", EstimatedDurationMinutes = 90, Status = SurgeryStatus.EM_PREPARACAO, Notes = "Paciente em jejum desde a véspera" }
        };
        context.Surgeries.AddRange(surgeries);

        // Payments (5)
        var payments = new List<Payment>
        {
            new() { PatientId = patients[0].Id, Description = "Consulta cardiológica", Amount = 250.00m, DueDate = today.AddDays(-5), PaymentDate = today.AddDays(-5), Method = PaymentMethod.PIX, Status = PaymentStatus.PAGO },
            new() { PatientId = patients[1].Id, Description = "Consulta clínica geral", Amount = 180.00m, DueDate = today.AddDays(5), Method = PaymentMethod.CONVENIO, Status = PaymentStatus.PENDENTE },
            new() { PatientId = patients[2].Id, Description = "Cirurgia ortopédica", Amount = 12500.00m, DueDate = today.AddDays(-10), PaymentDate = today.AddDays(-9), Method = PaymentMethod.CARTAO_CREDITO, Status = PaymentStatus.PAGO },
            new() { PatientId = patients[5].Id, Description = "Diária de internação", Amount = 3200.00m, DueDate = today.AddDays(-1), Method = PaymentMethod.BOLETO, Status = PaymentStatus.VENCIDO },
            new() { PatientId = patients[8].Id, Description = "Exames laboratoriais", Amount = 420.00m, DueDate = today.AddDays(10), Method = PaymentMethod.DINHEIRO, Status = PaymentStatus.PENDENTE }
        };
        context.Payments.AddRange(payments);

        // Medical Records (10)
        var medicalRecords = new List<MedicalRecord>
        {
            new() { PatientId = patients[0].Id, DoctorId = doctors[0].Id, Date = today.AddDays(-5), ChiefComplaint = "Dor no peito ocasional", History = "Paciente relata dor precordial leve há 2 semanas", Diagnosis = "Ansiedade / descartada causa cardíaca", Prescription = "Repouso e acompanhamento", Exams = "ECG normal" },
            new() { PatientId = patients[1].Id, DoctorId = doctors[4].Id, Date = today.AddDays(-8), ChiefComplaint = "Febre e dor de garganta", History = "Sintomas há 3 dias", Diagnosis = "Faringite viral", Prescription = "Sintomáticos e hidratação" },
            new() { PatientId = patients[2].Id, DoctorId = doctors[2].Id, Date = today.AddDays(-7), ChiefComplaint = "Dor intensa no fêmur após queda", History = "Acidente doméstico", Diagnosis = "Fratura de fêmur direito", Prescription = "Cirurgia de osteossíntese", Exams = "Raio-X confirma fratura" },
            new() { PatientId = patients[3].Id, DoctorId = doctors[3].Id, Date = today.AddDays(-3), ChiefComplaint = "Consulta pré-natal", History = "Gestação de 38 semanas", Diagnosis = "Gestação a termo", Prescription = "Programação de cesariana" },
            new() { PatientId = patients[4].Id, DoctorId = doctors[0].Id, Date = today.AddDays(-20), ChiefComplaint = "Dor torácica intensa", History = "Início súbito com sudorese", Diagnosis = "Infarto agudo do miocárdio", Prescription = "Internação em UTI e cateterismo", Exams = "ECG com supra de ST" },
            new() { PatientId = patients[5].Id, DoctorId = doctors[0].Id, Date = today.AddDays(-3), ChiefComplaint = "Pressão alta", History = "Histórico de hipertensão não controlada", Diagnosis = "Crise hipertensiva", Prescription = "Internação para monitoramento" },
            new() { PatientId = patients[6].Id, DoctorId = doctors[1].Id, Date = today, ChiefComplaint = "Vômitos e diarreia", History = "Sintomas há 2 dias, criança de 15 anos", Diagnosis = "Gastroenterite aguda", Prescription = "Hidratação venosa e dieta leve" },
            new() { PatientId = patients[7].Id, DoctorId = doctors[3].Id, Date = today.AddDays(-10), ChiefComplaint = "Exame de rotina", History = "Check-up anual", Diagnosis = "Sem alterações", Prescription = "Manter hábitos saudáveis", Exams = "Hemograma completo normal" },
            new() { PatientId = patients[8].Id, DoctorId = doctors[4].Id, Date = today.AddDays(-1), ChiefComplaint = "Glicemia descompensada", History = "Diabético tipo 2 há 10 anos", Diagnosis = "Descompensação diabética", Prescription = "Ajuste de insulina e internação" },
            new() { PatientId = patients[9].Id, DoctorId = doctors[0].Id, Date = today.AddDays(-2), ChiefComplaint = "Palpitações", History = "Episódios recorrentes de taquicardia", Diagnosis = "Arritmia a investigar", Prescription = "Cateterismo cardíaco agendado" }
        };
        context.MedicalRecords.AddRange(medicalRecords);

        await context.SaveChangesAsync();

        // Users
        var users = new List<User>
        {
            new() { Name = "Administrador do Sistema", Email = "admin@hospital.com", PasswordHash = passwordHasher.Hash("Admin@123"), Role = UserRole.ADMIN, IsActive = true },
            new() { Name = "Dr. Carlos Alberto Souza", Email = "carlos.souza@hospitalpromedico.com.br", PasswordHash = passwordHasher.Hash("Medico@123"), Role = UserRole.MEDICO, IsActive = true, DoctorId = doctors[0].Id },
            new() { Name = "Recepção Hospital ProMédico", Email = "recepcao@hospitalpromedico.com.br", PasswordHash = passwordHasher.Hash("Recepcao@123"), Role = UserRole.RECEPCAO, IsActive = true },
            new() { Name = "Financeiro Hospital ProMédico", Email = "financeiro@hospitalpromedico.com.br", PasswordHash = passwordHasher.Hash("Financeiro@123"), Role = UserRole.FINANCEIRO, IsActive = true }
        };
        context.Users.AddRange(users);

        await context.SaveChangesAsync();

        // Notifications
        var notifications = new List<Notification>
        {
            new() { UserId = users[0].Id, Title = "Bem-vindo!", Message = "Bem-vindo ao Hospital ProMédico. Seu sistema está pronto para uso.", Type = NotificationType.SUCESSO, Read = false },
            new() { UserId = users[0].Id, Title = "Pagamentos vencidos", Message = "Existem pagamentos vencidos que precisam de atenção.", Type = NotificationType.ALERTA, Read = false },
            new() { UserId = users[0].Id, Title = "Nova internação", Message = "Um novo paciente foi internado hoje.", Type = NotificationType.INFO, Read = false }
        };
        context.Notifications.AddRange(notifications);

        await context.SaveChangesAsync();
    }
}
