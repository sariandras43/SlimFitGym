import { MachineModel } from "./machine.model"

export interface RoomModel {
    id: number
    description: string
    recommendedPeople: number
    name: string
    isActive: boolean
    imageUrl: string
    machines: MachineModel[]
}