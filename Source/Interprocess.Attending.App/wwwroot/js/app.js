// Configurações da API
const API_BASE_URL = 'http://localhost:5117/api';

// Estado da aplicação
let patients = [];
let attendances = [];
let clinics = [];
let currentEditingPatient = null;
let currentEditingAttendance = null;

// INICIALIZAÇÃO
// ========================================

// Inicialização
document.addEventListener('DOMContentLoaded', function() {
    showSection('dashboard');
    loadDashboard();
    setupEventListeners();
});

// Event Listeners
function setupEventListeners() {
    // Busca de pacientes
    document.getElementById('patientSearch').addEventListener('input', debounce(searchPatients, 300));
    document.getElementById('patientCpfSearch').addEventListener('input', debounce(searchPatients, 300));
    document.getElementById('patientStatusFilter').addEventListener('change', searchPatients);
    
    // Busca de atendimentos
    document.getElementById('attendanceStartDate').addEventListener('change', searchAttendances);
    document.getElementById('attendanceEndDate').addEventListener('change', searchAttendances);
    document.getElementById('attendanceStatusFilter').addEventListener('change', searchAttendances);
}

// Utilitarios

function debounce(func, wait) {
    let timeout;
    return function executedFunction(...args) {
        const later = () => {
            clearTimeout(timeout);
            func(...args);
        };
        clearTimeout(timeout);
        timeout = setTimeout(later, wait);
    };
}

function showLoading(show = true) {
    const existingSpinner = document.querySelector('.spinner-overlay');
    if (show && !existingSpinner) {
        const spinner = document.createElement('div');
        spinner.className = 'spinner-overlay';
        spinner.innerHTML = '<div class="spinner-border" role="status"></div>';
        document.body.appendChild(spinner);
    } else if (!show && existingSpinner) {
        existingSpinner.remove();
    }
}

function showAlert(message, type = 'success') {
    const alertDiv = document.createElement('div');
    alertDiv.className = `alert alert-${type} alert-dismissible fade show alert-floating`;
    alertDiv.innerHTML = `
        ${message}
        <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
    `;
    document.body.appendChild(alertDiv);
    
    // Remove após 5 segundos
    setTimeout(() => {
        if (alertDiv.parentNode) {
            alertDiv.remove();
        }
    }, 5000);
}

function formatDate(dateString) {
    if (!dateString) return '';
    const date = new Date(dateString);
    return date.toLocaleDateString('pt-BR');
}

function formatDateTime(dateString) {
    if (!dateString) return '';
    const date = new Date(dateString);
    return date.toLocaleString('pt-BR');
}

function getStatusBadge(status) {
    return (status === 1 || status === "Active") ? 
        '<span class="badge bg-success">Ativo</span>' : 
        '<span class="badge bg-danger">Inativo</span>';
}

function getSexLabel(sex) {
    return sex === 1 ? 'Masculino' : 'Feminino';
}

// API

async function apiRequest(endpoint, method = 'GET', data = null) {
    const config = {
        method,
        headers: {
            'Content-Type': 'application/json',
        },
    };

    if (data) {
        config.body = JSON.stringify(data);
    }

    try {
        const response = await fetch(`${API_BASE_URL}${endpoint}`, config);
        
        if (!response.ok) {
            // Tentar obter detalhes do erro
            const errorText = await response.text();
            let errorMessage = `HTTP error! status: ${response.status}`;
            
            try {
                const errorData = JSON.parse(errorText);
                if (errorData.errors) {
                    errorMessage += '\n' + JSON.stringify(errorData.errors, null, 2);
                } else if (errorData.title) {
                    errorMessage = errorData.title;
                }
            } catch (e) {
                errorMessage += '\n' + errorText;
            }
            
            throw new Error(errorMessage);
        }

        // Verificar se a resposta tem conteúdo
        const contentType = response.headers.get('content-type');
        if (response.status === 204 || !contentType || !contentType.includes('application/json')) {
            return null; // Retorna null para respostas sem conteúdo
        }

        const responseText = await response.text();
        if (!responseText) {
            return null; // Retorna null se não há texto
        }

        return JSON.parse(responseText);
    } catch (error) {
        console.error('API Error:', error);
        console.error('Request data:', data);
        showAlert(`Erro na API: ${error.message}`, 'danger');
        throw error;
    }
}

// Navegação e Seções

function showSection(sectionName) {
    // Ocultar todas as seções
    document.querySelectorAll('.section').forEach(section => {
        section.classList.remove('active');
    });
    
    // Exibir a seção selecionada
    document.getElementById(sectionName).classList.add('active');
    
    // Atualizar navbar
    document.querySelectorAll('.nav-link').forEach(link => {
        link.classList.remove('active');
    });
    
    // Carregar conteúdo da seção
    switch(sectionName) {
        case 'dashboard':
            loadDashboard();
            break;
        case 'patients':
            loadPatients();
            break;
        case 'attendances':
            loadAttendances();
            break;
        case 'clinics':
            loadClinics();
            break;
    }
}

// ========================================
// DASHBOARD 
// ========================================
async function loadDashboard() {
    try {
        showLoading(true);
        
        // Carregar dados em paralelo
        const [patientsData, attendancesData, clinicsData] = await Promise.all([
            apiRequest('/patients'),
            apiRequest('/attendances'),
            apiRequest('/clinics')
        ]);
        
        // Atualizar contadores - verificar se os dados não são null
        document.getElementById('totalPatients').textContent = patientsData ? patientsData.length : 0;
        document.getElementById('totalAttendances').textContent = attendancesData ? attendancesData.length : 0;
        document.getElementById('totalClinics').textContent = clinicsData ? clinicsData.length : 0;
        
    } catch (error) {
        console.error('Erro ao carregar dashboard:', error);
    } finally {
        showLoading(false);
    }
}

// Funções de pacientes

async function loadPatients() {
    try {
        showLoading(true);
        const patientsData = await apiRequest('/patients');
        patients = patientsData || []; // Garantir que não seja null
        renderPatients(patients);
    } catch (error) {
        console.error('Erro ao carregar pacientes:', error);
    } finally {
        showLoading(false);
    }
}

function renderPatients(patientsData) {
    const tbody = document.getElementById('patientsTableBody');
    
    if (patientsData.length === 0) {
        tbody.innerHTML = `
            <tr>
                <td colspan="5" class="text-center empty-state">
                    <i class="bi bi-people"></i>
                    <p>Nenhum paciente encontrado</p>
                </td>
            </tr>
        `;
        return;
    }
    
    tbody.innerHTML = patientsData.map(patient => `
        <tr>
            <td>${patient.name}</td>
            <td>${patient.cpf}</td>
            <td>${formatDate(patient.dateBirth)}</td>
            <td>${getStatusBadge(patient.status)}</td>
            <td>
                <div class="action-buttons">
                    <button class="btn btn-sm btn-outline-primary" onclick="editPatient('${patient.id}')">
                        <i class="bi bi-pencil"></i>
                    </button>
                    <button class="btn btn-sm btn-outline-danger" onclick="deletePatient('${patient.id}')">
                        <i class="bi bi-trash"></i>
                    </button>
                </div>
            </td>
        </tr>
    `).join('');
}

async function searchPatients() {
    const name = document.getElementById('patientSearch').value;
    const cpf = document.getElementById('patientCpfSearch').value;
    const status = document.getElementById('patientStatusFilter').value;
    
    try {
        showLoading(true);
        
        const params = new URLSearchParams();
        if (name) params.append('name', name);
        if (cpf) params.append('cpf', cpf);
        if (status) params.append('status', status);
        
        const endpoint = params.toString() ? 
            `/patients/filters?${params.toString()}` : 
            '/patients';
            
        const filteredPatientsData = await apiRequest(endpoint);
        const filteredPatients = filteredPatientsData || []; // Garantir que não seja null
        renderPatients(filteredPatients);
        
    } catch (error) {
        console.error('Erro ao buscar pacientes:', error);
    } finally {
        showLoading(false);
    }
}

function showPatientModal(patientId = null) {
    currentEditingPatient = patientId;
    const modal = new bootstrap.Modal(document.getElementById('patientModal'));
    
    // Limpar formulário
    document.getElementById('patientForm').reset();
    document.getElementById('patientId').value = '';
    
    if (patientId) {
        // Carregar dados do paciente
        const patient = patients.find(p => p.id === patientId);
        if (patient) {
            document.getElementById('patientId').value = patient.id;
            document.getElementById('patientName').value = patient.name;
            document.getElementById('patientCpf').value = patient.cpf;
            document.getElementById('patientDateBirth').value = patient.dateBirth ? patient.dateBirth.split('T')[0] : '';
            document.getElementById('patientSex').value = patient.sex;
            document.getElementById('patientStreet').value = patient.street;
            document.getElementById('patientCity').value = patient.city;
            document.getElementById('patientState').value = patient.state;
            document.getElementById('patientZipCode').value = patient.zipCode;
            document.getElementById('patientDistrict').value = patient.district;
            document.getElementById('patientComplement').value = patient.complement || '';
        }
    }
    
    modal.show();
}

async function savePatient() {
    const form = document.getElementById('patientForm');
    if (!form.checkValidity()) {
        form.classList.add('was-validated');
        return;
    }
    
    const patientData = {
        name: document.getElementById('patientName').value,
        cpf: document.getElementById('patientCpf').value,
        dateBirth: document.getElementById('patientDateBirth').value,
        sex: document.getElementById('patientSex').value, // Mantém como string
        street: document.getElementById('patientStreet').value,
        city: document.getElementById('patientCity').value,
        state: document.getElementById('patientState').value,
        zipCode: document.getElementById('patientZipCode').value,
        district: document.getElementById('patientDistrict').value,
        complement: document.getElementById('patientComplement').value || ''
    };
    
    try {
        showLoading(true);
        
        if (currentEditingPatient) {
            // Atualizar paciente
            await apiRequest(`/patients/${currentEditingPatient}`, 'PUT', patientData);
            showAlert('Paciente atualizado com sucesso!');
        } else {
            // Criar paciente
            await apiRequest('/patients', 'POST', patientData);
            showAlert('Paciente criado com sucesso!');
        }
        
        // Fechar modal e recarregar lista
        bootstrap.Modal.getInstance(document.getElementById('patientModal')).hide();
        loadPatients();
        
    } catch (error) {
        console.error('Erro ao salvar paciente:', error);
        showAlert('Erro ao salvar paciente', 'danger');
    } finally {
        showLoading(false);
    }
}

function editPatient(patientId) {
    showPatientModal(patientId);
}

async function deletePatient(patientId) {
    if (!confirm('Tem certeza que deseja inativar este paciente?')) {
        return;
    }
    
    try {
        showLoading(true);
        await apiRequest(`/patients/${patientId}`, 'DELETE');
        showAlert('Paciente inativado com sucesso!');
        loadPatients();
    } catch (error) {
        console.error('Erro ao inativar paciente:', error);
        showAlert('Erro ao inativar paciente', 'danger');
    } finally {
        showLoading(false);
    }
}

// Funções de atendimentos

// Carrega todos os atendimentos da API
async function loadAttendances() {
    try {
        showLoading(true);
        const attendancesData = await apiRequest('/attendances');
        attendances = attendancesData || []; // Garantir que não seja null
        renderAttendances(attendances);
    } catch (error) {
        console.error('Erro ao carregar atendimentos:', error);
    } finally {
        showLoading(false);
    }
}

// Renderiza a lista de atendimentos na tabela
function renderAttendances(attendancesData) {
    const tbody = document.getElementById('attendancesTableBody');
    
    if (attendancesData.length === 0) {
        tbody.innerHTML = `
            <tr>
                <td colspan="6" class="text-center empty-state">
                    <i class="bi bi-calendar-check"></i>
                    <p>Nenhum atendimento encontrado</p>
                </td>
            </tr>
        `;
        return;
    }
    
    // Gera HTML para cada atendimento
    tbody.innerHTML = attendancesData.map(attendance => `
        <tr>
            <td>${formatDateTime(attendance.createdOnUtc)}</td>
            <td>${attendance.patientName || 'N/A'}</td>
            <td>${attendance.clinicName || 'N/A'}</td>
            <td>${attendance.description}</td>
            <td>${getStatusBadge(attendance.status)}</td>
            <td>
                <div class="action-buttons">
                    <button class="btn btn-sm btn-outline-primary" onclick="editAttendance('${attendance.id}')">
                        <i class="bi bi-pencil"></i>
                    </button>
                    <button class="btn btn-sm btn-outline-danger" onclick="deleteAttendance('${attendance.id}')">
                        <i class="bi bi-trash"></i>
                    </button>
                </div>
            </td>
        </tr>
    `).join('');
}

// Busca atendimentos com filtros
async function searchAttendances() {
    const startDate = document.getElementById('attendanceStartDate').value;
    const endDate = document.getElementById('attendanceEndDate').value;
    const status = document.getElementById('attendanceStatusFilter').value;
    
    try {
        showLoading(true);
        
        const params = new URLSearchParams();
        if (startDate) params.append('startDate', startDate);
        if (endDate) params.append('endDate', endDate);
        if (status) params.append('status', status);
        
        const endpoint = params.toString() ? 
            `/attendances/filters?${params.toString()}` : 
            '/attendances';
            
        const filteredAttendances = await apiRequest(endpoint);
        renderAttendances(filteredAttendances);
        
    } catch (error) {
        console.error('Erro ao buscar atendimentos:', error);
    } finally {
        showLoading(false);
    }
}

async function showAttendanceModal(attendanceId = null) {
    currentEditingAttendance = attendanceId;
    const modal = new bootstrap.Modal(document.getElementById('attendanceModal'));
    
    // Limpar formulário
    document.getElementById('attendanceForm').reset();
    document.getElementById('attendanceId').value = '';
    
    try {
        showLoading(true);
        
        // Carregar clínicas e pacientes para os selects
        const [clinicsData, patientsData] = await Promise.all([
            apiRequest('/clinics'),
            apiRequest('/patients')
        ]);
        
        // Preencher select de clínicas
        const clinicSelect = document.getElementById('attendanceClinic');
        clinicSelect.innerHTML = '<option value="">Selecione uma clínica</option>';
        clinicsData.forEach(clinic => {
            clinicSelect.innerHTML += `<option value="${clinic.id}">${clinic.name}</option>`;
        });
        
        // Preencher select de pacientes
        const patientSelect = document.getElementById('attendancePatient');
        patientSelect.innerHTML = '<option value="">Selecione um paciente</option>';
        patientsData.forEach(patient => {
            patientSelect.innerHTML += `<option value="${patient.id}">${patient.name}</option>`;
        });
        
        // Se está editando, carregar dados do atendimento
        if (attendanceId) {
            const attendance = attendances.find(a => a.id === attendanceId);
            if (attendance) {
                document.getElementById('attendanceId').value = attendance.id;
                document.getElementById('attendanceClinic').value = attendance.clinicId;
                document.getElementById('attendancePatient').value = attendance.patientId;
                document.getElementById('attendanceDescription').value = attendance.description;
                
                // Formatar data para datetime-local
                if (attendance.startedDate) {
                    const date = new Date(attendance.startedDate);
                    const localDate = new Date(date.getTime() - date.getTimezoneOffset() * 60000);
                    document.getElementById('attendanceStartedDate').value = localDate.toISOString().slice(0, 16);
                }
            }
        }
        
        modal.show();
        
    } catch (error) {
        console.error('Erro ao carregar dados do modal:', error);
        showAlert('Erro ao carregar dados', 'danger');
    } finally {
        showLoading(false);
    }
}

async function saveAttendance() {
    const form = document.getElementById('attendanceForm');
    if (!form.checkValidity()) {
        form.classList.add('was-validated');
        return;
    }
    
    const attendanceData = {
        clinicId: document.getElementById('attendanceClinic').value,
        patientId: document.getElementById('attendancePatient').value,
        description: document.getElementById('attendanceDescription').value,
        startedDate: document.getElementById('attendanceStartedDate').value
    };
    
    try {
        showLoading(true);
        
        if (currentEditingAttendance) {
            // Atualizar atendimento
            await apiRequest(`/attendances/${currentEditingAttendance}`, 'PUT', attendanceData);
            showAlert('Atendimento atualizado com sucesso!');
        } else {
            // Criar atendimento
            await apiRequest('/attendances', 'POST', attendanceData);
            showAlert('Atendimento criado com sucesso!');
        }
        
        // Fechar modal e recarregar lista
        bootstrap.Modal.getInstance(document.getElementById('attendanceModal')).hide();
        loadAttendances();
        
    } catch (error) {
        console.error('Erro ao salvar atendimento:', error);
        showAlert('Erro ao salvar atendimento', 'danger');
    } finally {
        showLoading(false);
    }
}

function editAttendance(attendanceId) {
    showAttendanceModal(attendanceId);
}

async function deleteAttendance(attendanceId) {
    if (!confirm('Tem certeza que deseja inativar este atendimento?')) {
        return;
    }
    
    try {
        showLoading(true);
        await apiRequest(`/attendances/${attendanceId}`, 'DELETE');
        showAlert('Atendimento inativado com sucesso!');
        loadAttendances();
    } catch (error) {
        console.error('Erro ao inativar atendimento:', error);
        showAlert('Erro ao inativar atendimento', 'danger');
    } finally {
        showLoading(false);
    }
}

// Funções da clinica

async function loadClinics() {
    try {
        showLoading(true);
        const clinicsData = await apiRequest('/clinics');
        clinics = clinicsData || []; // Garantir que não seja null
        renderClinics(clinics);
    } catch (error) {
        console.error('Erro ao carregar clínicas:', error);
    } finally {
        showLoading(false);
    }
}

function renderClinics(clinicsData) {
    const tbody = document.getElementById('clinicsTableBody');
    
    if (clinicsData.length === 0) {
        tbody.innerHTML = `
            <tr>
                <td colspan="2" class="text-center empty-state">
                    <i class="bi bi-building"></i>
                    <p>Nenhuma clínica encontrada</p>
                </td>
            </tr>
        `;
        return;
    }
    
    tbody.innerHTML = clinicsData.map(clinic => `
        <tr>
            <td>${clinic.name}</td>
            <td>${clinic.id}</td>
        </tr>
    `).join('');
}
