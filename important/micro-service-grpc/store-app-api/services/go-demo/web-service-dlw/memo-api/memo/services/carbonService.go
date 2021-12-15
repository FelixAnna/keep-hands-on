package services

import (
	"github.com/golang-module/carbon/v2"
)

var CarbonTimeMap map[int]int
var LunarTimeMap map[int]int

const start = 19010101
const end = 20501231

func init() {
	//init 1901-2050 carbon and lunar maps
	length := (start - end) / 10000 * 365
	CarbonTimeMap = make(map[int]int, length)
	LunarTimeMap = make(map[int]int, length)
	initMap()
}

func initMap() {
	startCarbon := getCarbonDate(start)
	endCarbon := getCarbonDate(end)

	i, j := 0, 0
	for startCarbon.Lte(*endCarbon) {
		lunar := startCarbon.Lunar()

		carbonKey := startCarbon.Year()*10000 + startCarbon.Month()*100 + startCarbon.Day()
		lunarKey := lunar.Year()*10000 + lunar.Month()*100 + lunar.Day()
		CarbonTimeMap[carbonKey] = i
		LunarTimeMap[lunarKey] = j

		i, j = i+1, j+1

		startCarbonNew := startCarbon.AddDay()
		startCarbon = &startCarbonNew
	}
}

func GetCarbonDistanceWithCacheAside(startDate, targetDate int) (before, after int64) {
	targetValue, ok := CarbonTimeMap[targetDate]
	if ok {
		_, startMonthDay := startDate/10000, startDate%10000
		targetYear, targetMonthDay := targetDate/10000, targetDate%10000

		if startMonthDay < targetMonthDay {
			//targetYear + startMonthDay
			//targetYear+1 + startMonthDay
			preDate, nextDate := targetYear*10000+startMonthDay, (targetYear+1)*10000+startMonthDay
			preDateValue, okPre := CarbonTimeMap[preDate]
			nextDateValue, okNext := CarbonTimeMap[nextDate]
			if okPre && okNext {
				before = int64(preDateValue) - int64(targetValue)
				after = int64(nextDateValue) - int64(targetValue)
				return
			}

		} else if startMonthDay > targetMonthDay {
			//targetYear-1 + startMonthDay
			//targetYear + startMonthDay
			preDate, nextDate := (targetYear-1)*10000+startMonthDay, targetYear*10000+startMonthDay
			preDateValue, okPre := CarbonTimeMap[preDate]
			nextDateValue, okNext := CarbonTimeMap[nextDate]
			if okPre && okNext {
				before = int64(preDateValue) - int64(targetValue)
				after = int64(nextDateValue) - int64(targetValue)
				return
			}

		} else {
			return 0, 0
		}
	}

	return GetCarbonDistance(startDate, targetDate)
}

func GetLunarDistanceWithCacheAside(startDate, targetDate int) (before, after int64) {
	startCarbon := getCarbonDate(startDate)
	targetCarbon := getCarbonDate(targetDate)

	startLunarDate := startCarbon.Lunar()
	targetLunarDate := targetCarbon.Lunar()

	startDate = startLunarDate.Year()*10000 + startLunarDate.Month()*100 + startLunarDate.Day()
	targetDate = targetLunarDate.Year()*10000 + targetLunarDate.Month()*100 + targetLunarDate.Day()

	targetValue, ok := CarbonTimeMap[targetDate]
	if ok {
		_, startMonthDay := startDate/10000, startDate%10000
		targetYear, targetMonthDay := targetDate/10000, targetDate%10000

		if startMonthDay < targetMonthDay {
			//targetYear + startMonthDay
			//targetYear+1 + startMonthDay
			preDate, nextDate := targetYear*10000+startMonthDay, (targetYear+1)*10000+startMonthDay
			preDateValue, okPre := CarbonTimeMap[preDate]
			nextDateValue, okNext := CarbonTimeMap[nextDate]
			if okPre && okNext {
				before = int64(preDateValue) - int64(targetValue)
				after = int64(nextDateValue) - int64(targetValue)
				return
			}

		} else if startMonthDay > targetMonthDay {
			//targetYear-1 + startMonthDay
			//targetYear + startMonthDay
			preDate, nextDate := (targetYear-1)*10000+startMonthDay, targetYear*10000+startMonthDay
			preDateValue, okPre := CarbonTimeMap[preDate]
			nextDateValue, okNext := CarbonTimeMap[nextDate]
			if okPre && okNext {
				before = int64(preDateValue) - int64(targetValue)
				after = int64(nextDateValue) - int64(targetValue)
				return
			}

		} else {
			return 0, 0
		}
	}

	return GetLunarDistance(startDate, targetDate)
}

/*
GetCarbonDistance - Get the distance between startDate and targetDate (ignore year)
Suppose target date is now,
return how many days before and how many days later if startDate (same month and day)
*/
func GetCarbonDistance(startDate, targetDate int) (before, after int64) {

	startCarbon := getCarbonDate(startDate)
	targetCarbon := getCarbonDate(targetDate)

	diffYear := startCarbon.DiffInYears(*targetCarbon)
	startCarbonThisYear := startCarbon.AddYears(int(diffYear))
	diffDays := targetCarbon.DiffInDays(startCarbonThisYear)

	if diffDays < 0 { //target after start - n days before were start, then find m days later when it will be start again
		before = diffDays

		startCarbonNextYear := startCarbonThisYear.AddYear()
		after = targetCarbon.DiffInDays(startCarbonNextYear)
	} else if diffDays > 0 { //target before start - n days later will be start, then find m days before when it was start
		after = diffDays

		startCarbonPreYear := startCarbonThisYear.SubYear()
		before = targetCarbon.DiffInDays(startCarbonPreYear)
	} else {
		return 0, 0
	}

	return
}

func GetLunarDistance(startDate, targetDate int) (before, after int64) {
	startCarbon := getCarbonDate(startDate)
	targetCarbon := getCarbonDate(targetDate)

	before = getLunarDistance(startCarbon, targetCarbon, false)
	after = getLunarDistance(startCarbon, targetCarbon, true)
	return
}

func getLunarDistance(startCarbon, targetCarbon *carbon.Carbon, forward bool) int64 {
	distance := 0
	startLunarDate := startCarbon.Lunar()
	targetLunarDate := targetCarbon.Lunar()

	startMMdd := startLunarDate.Month()*100 + startLunarDate.Day()
	targetMMdd := targetLunarDate.Month()*100 + targetLunarDate.Day()
	for startMMdd != targetMMdd {
		if forward {
			targetCarbonNew := targetCarbon.AddDays(1)
			targetCarbon = &targetCarbonNew

			targetLunarDate = targetCarbon.Lunar()
			targetMMdd = targetLunarDate.Month()*100 + targetLunarDate.Day()
			distance += 1
		} else {
			targetCarbonNew := targetCarbon.AddDays(-1)
			targetCarbon = &targetCarbonNew

			targetLunarDate = targetCarbon.Lunar()
			targetMMdd = targetLunarDate.Month()*100 + targetLunarDate.Day()
			distance -= 1
		}
	}

	return int64(distance)
}

func getCarbonDate(date int) *carbon.Carbon {
	carbonDate := carbon.CreateFromDate(date/10000, (date%10000)/100, date%100)
	return &carbonDate
}
