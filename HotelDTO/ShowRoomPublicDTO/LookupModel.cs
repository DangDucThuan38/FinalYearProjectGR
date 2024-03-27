namespace DangDucThuanFinalYear.HotelDTO.ShowRoomPublicDTO
{
    public readonly record struct LookupModel<TId>(TId Id, string name);
    public readonly record struct LookupModel<TId, TAdditionalData>(TId Id, string name, TAdditionalData AdditionalData)
    {
        public bool IsEmpty => string.IsNullOrWhiteSpace(name);
    }

}
