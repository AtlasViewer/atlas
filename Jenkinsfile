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
        stage("Build on Windows") {
            agent {
                label 'windows'
            }

            steps {
                script {
                    bat '''
                    @echo off

                    Unity -quit -projectPath "%cd%" -batchmode -buildTarget win64 -customBuildPath "%cd%\\out\\" -customBuildName "AtlasViewer" -executeMethod BuildCommand.ProcessCLIBuild
                    '''
                }
            }
        }
    }
}
