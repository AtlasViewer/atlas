pipeline {
    agent any

    options {
        timestamps()
        buildDiscarder(logRotator(numToKeepStr: '5'))
    }

    environment {
        IS_DEVELOPMENT_BUILD = true // Temporarily, all builds are dev builds. In the future only the dev branch will be development builds
        // A second flag will be added on first initial release: SNAPSHOT. Which will indicate if the viewer is a snapshot of the live branch or a build issued by the team.
    }

    stages {
        stage("Clean Environment") {
            agent {
                label 'windows'
            }

            steps {
                script {
                    bat '''

                    git clean -xfd
                    git reset --hard

                    mkdir out
                    mkdir out\\windows
                    mkdir out\\linux
                    mkdir out\\Android
                    mkdir out\\macOS
                    '''
                }
            }
        }
        stage("Build Windows") {
            agent {
                label 'windows'
            }

            environment {
                BUILD_NAME = "Windows-${currentBuild.number}"
                String buildTarget = "Win64"
            }

            steps {
                script {
                    bat '''

                    Unity -quit -projectPath "%cd%" -logfile - -batchmode -buildTarget %buildTarget% -outputPath "%cd%" -executeMethod BuildCommand.PerformWindows
                    '''
                }
            }
        }

        stage("Build Linux") {
            agent {
                label 'windows'
            }

            environment {
                BUILD_NAME = "Linux-${currentBuild.number}"
                String buildTarget = "Linux64"
            }

            steps {
                script {
                    bat '''

                    Unity -quit -projectPath "%cd%" -logfile - -batchmode -buildTarget %buildTarget% -outputPath "%cd%" -executeMethod BuildCommand.PerformLinux
                    '''
                }
            }
        }

        stage("Build MacOS") {
            agent {
                label 'windows'
            }

            environment {
                BUILD_NAME = "MacOS-${currentBuild.number}"
                String buildTarget = "macOS"
            }

            steps {
                script {
                    bat '''

                    Unity -quit -projectPath "%cd%" -logfile - -batchmode -buildTarget %buildTarget% -outputPath "%cd%" -executeMethod BuildCommand.PerformMacOS
                    '''
                }
            }
        }

        stage("Build Android") {
            agent {
                label 'windows'
            }

            environment {
                BUILD_NAME = "Android-${currentBuild.number}"
                String buildTarget = "Android"
            }

            steps {
                script {
                    bat '''

                    Unity -quit -projectPath "%cd%" -logfile - -batchmode -buildTarget %buildTarget% -outputPath "%cd%" -executeMethod BuildCommand.PerformAndroid
                    '''
                }
            }
        }
    }
}
