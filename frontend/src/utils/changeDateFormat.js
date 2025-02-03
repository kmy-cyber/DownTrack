export function convertDateFormat(dateString) {

    // Parse the input string
    const cleanedDateString = dateString.replace('T', ' ');
    const [year, month, day] = cleanedDateString.split('-');
    const onlyDay = day.split(' ')[0];
  // Format the date as DD-MM-YYYY
    const formattedDate = `${onlyDay}-${month}-${year}`;
    console.log("onlyDay",onlyDay);
    console.log("month",month);
    console.log("year",year);
    console.log("sin T",cleanedDateString);
    console.log("formattDate:",formattedDate);

  // Append the time
    return `${formattedDate}`;
}
