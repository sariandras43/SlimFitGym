export interface TrainingModel {
    id: number
    trainer: string
    trainerId: number
    room: string
    roomId: number
    name: string
    trainingStart: string
    trainingEnd: string
    maxPeople: number
    freePlaces: number
    isActive: boolean
    trainerImageUrl: string
    roomImageUrl: string
  }
  