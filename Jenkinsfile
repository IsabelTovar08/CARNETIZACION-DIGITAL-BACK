/// <summary>
/// Jenkinsfile principal para despliegue automatizado del proyecto carnetizacion-digital-api.
/// Este pipeline detecta el entorno desde el archivo .env raíz,
/// compila el proyecto .NET 8 y ejecuta el docker-compose correspondiente dentro de la carpeta devops/{entorno}.
/// Antes del despliegue, elimina cualquier contenedor previo con el mismo nombre para evitar conflictos.
/// </summary>

pipeline {
    /// <summary>
    /// Define el agente que ejecutará el pipeline. 
    /// En este caso 'any' indica que puede correr en cualquier nodo disponible de Jenkins.
    /// </summary>
    agent any

    /// <summary>
    /// Variables de entorno globales usadas durante todo el pipeline.
    /// Configura el comportamiento de .NET CLI y evita logs innecesarios o errores de permisos.
    /// </summary>
    environment {
        DOTNET_CLI_HOME = '/var/jenkins_home/.dotnet'
        DOTNET_SKIP_FIRST_TIME_EXPERIENCE = '1'
        DOTNET_NOLOGO = '1'
    }

    stages {

        /// <summary>
        /// Etapa 1: Detección del entorno.
        /// Lee el archivo .env en la raíz del proyecto para obtener la variable ENVIRONMENT (por ejemplo, qa, prod, etc.)
        /// Luego construye las rutas de los archivos docker-compose y .env correspondientes dentro de devops/.
        /// </summary>
        stage('Leer entorno desde .env raíz') {
            steps {
                script {
                    // Extraer valor de ENVIRONMENT del archivo .env (por ejemplo ENVIRONMENT=qa)
                    def envValue = sh(script: "grep '^ENVIRONMENT=' .env | cut -d '=' -f2", returnStdout: true).trim()

                    // Validar que exista la variable
                    if (envValue == '') {
                        error "No se encontró ENVIRONMENT en el archivo .env raíz"
                    }

                    // Asignar variables dinámicas para las rutas de configuración
                    env.ENVIRONMENT = envValue
                    env.ENV_DIR = "devops/${env.ENVIRONMENT}"
                    env.COMPOSE_FILE = "${env.ENV_DIR}/docker-compose.yml"
                    env.ENV_FILE = "${env.ENV_DIR}/.env"

                    // Mostrar información en consola Jenkins
                    echo "Entorno detectado: ${env.ENVIRONMENT}"
                    echo "Archivo compose: ${env.COMPOSE_FILE}"
                    echo "Archivo de entorno: ${env.ENV_FILE}"
                }
            }
        }

        /// <summary>
        /// Etapa 2: Restauración de dependencias.
        /// Se ejecuta dentro de un contenedor oficial de .NET SDK 8.0.
        /// Restaura los paquetes NuGet necesarios para compilar la solución.
        /// </summary>
        stage('Restaurar dependencias') {
            agent {
                docker {
                    image 'mcr.microsoft.com/dotnet/sdk:8.0'
                    args '-v /var/run/docker.sock:/var/run/docker.sock'
                }
            }
            steps {
                sh '''
                    # Crear el directorio para .NET CLI y asignar permisos
                    mkdir -p $DOTNET_CLI_HOME
                    chmod -R 777 $DOTNET_CLI_HOME
                    
                    # Restaurar dependencias del proyecto principal
                    dotnet restore Web/Web.csproj
                '''
            }
        }

        /// <summary>
        /// Etapa 3: Compilación del proyecto.
        /// Usa el SDK de .NET 8.0 para compilar el proyecto Web en configuración Release.
        /// </summary>
        stage('Compilar proyecto') {
            agent {
                docker { image 'mcr.microsoft.com/dotnet/sdk:8.0' }
            }
            steps {
                echo 'Compilando la solución carnetizacion-digital-api...'
                sh 'dotnet build Web/Web.csproj --configuration Release'
            }
        }

        /// <summary>
        /// Etapa 4: Despliegue del backend.
        /// Ejecuta el docker-compose del entorno correspondiente para construir e iniciar el contenedor del backend.
        /// Antes de levantarlo, limpia cualquier contenedor previo con el mismo nombre para evitar conflictos.
        /// </summary>
        stage('Desplegar API') {
            steps {
                echo "Desplegando carnetizacion-digital-api para entorno: ${env.ENVIRONMENT}"
                
                sh """
                    # Eliminar contenedores previos del entorno si existen
                    echo "Limpiando contenedores antiguos para ${env.ENVIRONMENT}..."
                    docker ps -a --filter "name=carnetizacion-digital-api-${env.ENVIRONMENT}" -q | xargs -r docker rm -f || true

                    # Ejecutar docker-compose correspondiente al entorno detectado
                    docker compose -f ${env.COMPOSE_FILE} --env-file ${env.ENV_FILE} up -d --build
                """
            }
        }
    }

    /// <summary>
    /// Bloque final del pipeline. 
    /// Define acciones a realizar según el resultado del proceso (éxito o fallo).
    /// </summary>
    post {
        success {
            echo "Despliegue completado correctamente para ${env.ENVIRONMENT}"
        }
        failure {
            echo "Error durante el despliegue en ${env.ENVIRONMENT}"
        }
    }
}