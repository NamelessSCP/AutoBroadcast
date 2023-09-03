# AutoBroadcast ![Downloads](https://img.shields.io/github/downloads/Misfiy/AutoBroadcast/total)
Automatically broadcast a message through the facility at specific times

# Support
* For any issues create an issue or contact me on [Discord](https://discord.gg/RYzahv3vfC).
* Also use the Discord for suggestions :)

# Default Config
```yaml
AutoBroadcast:
  is_enabled: true
  debug: false
  join_message:
  # How long the hint/broadcast should show
    duration: 4
    # The message shown on the broadcast
    message: 'Welcome, %name%!'
    # Override broadcasts
    override_broadcasts: false
  chaos_announcement:
  # The CASSIE message to be sent
    message: 'Warning . Military Personnel has entered the facility . Designated as, Chaos Insurgency.'
    # The text to be shown
    translation: 'Warning. Military Personnel has entered the facility. Designated as, Chaos Insurgency.'
    # Whether or not to hide the subtitle for the cassie message
    show_subtitles: false
  cassie_round_start:
  # The CASSIE message to be sent
    message: 'Containment breach'
    # The text to be shown
    translation: 'Containment breach!'
    # Whether or not to hide the subtitle for the cassie message
    show_subtitles: false
  broadcasts:
    10:
    # How long the hint/broadcast should show
      duration: 5
      # The message shown on the broadcast
      message: '10 seconds have passed!'
      # Override broadcasts
      override_broadcasts: false
    60:
    # How long the hint/broadcast should show
      duration: 6
      # The message shown on the broadcast
      message: '60 seconds have passed!'
      # Override broadcasts
      override_broadcasts: false
  broadcast_intervals:
    15:
    # How long the hint/broadcast should show
      duration: 3
      # The message shown on the broadcast
      message: 'Every 15 seconds!'
      # Override broadcasts
      override_broadcasts: false
    120:
    # How long the hint/broadcast should show
      duration: 4
      # The message shown on the broadcast
      message: 'Every 120 seconds!'
      # Override broadcasts
      override_broadcasts: false
  spawn_broadcasts:
    Spectator:
      message: 'You died!'
      duration: 4
      override: false
      use_hints: true
    Overwatch:
      message: 'Overwatch initiated.'
      duration: 6
      override: false
      use_hints: true
```
# Empty config
```yaml
AutoBroadcast:
  is_enabled: true
  debug: false
  join_message:
  # How long the hint/broadcast should show
    duration: 0
    # The message shown on the broadcast
    message: ''
    # Override broadcasts
    override_broadcasts: false
  chaos_announcement:
  # The CASSIE message to be sent
    message: ""
    # The text to be shown
    translation: ""
    # Whether or not to hide the subtitle for the cassie message
    show_subtitles: false
  cassie_round_start:
  # The CASSIE message to be sent
    message: ""
    # The text to be shown
    translation: ""
    # Whether or not to hide the subtitle for the cassie message
    show_subtitles: false
  broadcasts: {}
  broadcast_intervals: {}
  spawn_broadcasts: {}
```
