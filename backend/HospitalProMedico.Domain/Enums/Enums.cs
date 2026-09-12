namespace HospitalProMedico.Domain.Enums;

public enum UserRole
{
    ADMIN = 1,
    MEDICO = 2,
    RECEPCAO = 3,
    FINANCEIRO = 4
}

public enum Gender
{
    MASCULINO = 1,
    FEMININO = 2,
    OUTRO = 3
}

public enum BloodType
{
    A_POSITIVO = 1,
    A_NEGATIVO = 2,
    B_POSITIVO = 3,
    B_NEGATIVO = 4,
    AB_POSITIVO = 5,
    AB_NEGATIVO = 6,
    O_POSITIVO = 7,
    O_NEGATIVO = 8
}

public enum RegisterStatus
{
    ATIVO = 1,
    INATIVO = 2
}

public enum Specialty
{
    CARDIOLOGIA = 1,
    CLINICA_GERAL = 2,
    PEDIATRIA = 3,
    ORTOPEDIA = 4,
    NEUROLOGIA = 5,
    GINECOLOGIA = 6,
    DERMATOLOGIA = 7,
    OFTALMOLOGIA = 8,
    CIRURGIA_GERAL = 9
}

public enum AppointmentType
{
    PRIMEIRA_CONSULTA = 1,
    RETORNO = 2,
    URGENCIA = 3,
    EXAME = 4
}

public enum AppointmentStatus
{
    AGENDADA = 1,
    CONFIRMADA = 2,
    REALIZADA = 3,
    CANCELADA = 4
}

public enum HospitalizationType
{
    CLINICA = 1,
    CIRURGICA = 2,
    UTI = 3,
    OBSERVACAO = 4
}

public enum HospitalizationStatus
{
    INTERNADO = 1,
    ALTA = 2,
    CANCELADO = 3
}

public enum SurgeryStatus
{
    AGENDADA = 1,
    EM_PREPARACAO = 2,
    REALIZADA = 3,
    CANCELADA = 4
}

public enum HealthPlanType
{
    BASICO = 1,
    INTERMEDIARIO = 2,
    PREMIUM = 3,
    EMPRESARIAL = 4
}

public enum PaymentMethod
{
    DINHEIRO = 1,
    PIX = 2,
    CARTAO_CREDITO = 3,
    CARTAO_DEBITO = 4,
    BOLETO = 5,
    CONVENIO = 6
}

public enum PaymentStatus
{
    PENDENTE = 1,
    PAGO = 2,
    VENCIDO = 3,
    CANCELADO = 4
}

public enum NotificationType
{
    INFO = 1,
    ALERTA = 2,
    SUCESSO = 3,
    ERRO = 4
}
