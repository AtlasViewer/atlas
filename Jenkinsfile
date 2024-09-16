pipeline {
    agent any

    options {
        timestamps()
        buildDiscarder(logRotator(numToKeepStr: '5'))
    }

    stages {
        stage("Build on Windows") {
            agent {
                label 'windows'
            }

            steps {
                script {
                    bat '''
                    @echo off

                    Unity -quit -projectPath "%cd%" -batchmode -buildTarget win64
                    '''
                }
            }
        }
    }
}
