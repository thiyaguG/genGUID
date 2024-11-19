# The structure of a GUID

The format is 16 octets (128 bits) that conform to a specific structure. To make matters confusing there's a few different structures but the most common one is the structure defined in RFC 4122 - and that's the structure we'll be using for our GUID guide today. The structure is determined by the "variant" which is specified in the most significant bits of the 8th octet. We'll cover off setting this later on in the guide.

### The variant of GUID we're creating defines the following fields


- Octet 0-3: time_low The low field of the timestamp
- Octet 4-5: time_mid The middle field of the timestamp
- Octet 6-7: time_hi_and_version The high field of the timestamp multiplexed with the version number
- Octet 8: clock_seq_hi_and_reserved The high field of the clock sequence multiplexed with the variant
- Octet 9: clock_seq_low The low field of the clock sequence
- Octet 10-15: node The spatially unique node identifier